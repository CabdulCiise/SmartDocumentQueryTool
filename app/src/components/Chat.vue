<template>
  <div class="chat">
    <div class="chat__content">
      <div class="chat__inner">
        <div class="chat__messages" ref="chatMessages">
          <template v-for="message in messages">
            <div class="chat__bubble">
              <label class="chat__prompt">{{ message.prompt }}</label>
            </div>
            <div v-if="message.response" class="chat__bubble">
              <label class="chat__response">{{ message.response }}</label>
            </div>
          </template>
        </div>
      </div>
      <div class="chat__bar">
        <span class="chat__input">
          <InputText
            v-model="prompt"
            autofocus
            placeholder="What do you want to ask?"
            type="input"
            @keydown.enter="onNewPrompt"
          />
          <i class="pi pi-arrow-circle-up"></i>
        </span>
        <Button
          icon="pi pi-trash"
          text
          serverity="danger"
          :disabled="!messages || messages.length == 0"
          v-tooltip.top="'Delete Chat'"
          class="p-button-danger"
          @click="onDeleteChat"
        />
        <Button
          icon="pi pi-refresh"
          text
          :disabled="!messages || messages.length == 0"
          v-tooltip.top="'New Chat'"
          @click="onChatReset"
        />
        <Button
          icon="pi pi-thumbs-up"
          text
          v-tooltip.left="'Leave Feedback'"
          @click="onShowLeaveFeedback"
        />
      </div>
    </div>
  </div>
  <LeaveFeedback :visible="showLeaveFeedback" :user-id="userId" />
</template>

<script>
import api from '../api/api';

import Button from 'primevue/button';
import InputText from 'primevue/inputtext';

import LeaveFeedback from '../components/LeaveFeedback.vue';
import { nextTick } from 'vue';
import { showNotification } from '../services/notification';

export default {
  props: {
    userId: { name: 'user-id', required: true },
    chatId: { name: 'chat-id', required: true },
  },
  emits: ['chat-reset', 'chat-deleted', 'new-chat-prompted'],
  components: {
    Button,
    InputText,
    LeaveFeedback,
  },
  data() {
    return {
      isHandlingAPrompt: false,
      prompt: '',
      messages: [],
      showLeaveFeedback: false,
    };
  },
  methods: {
    async onNewPrompt() {
      if (!this.isHandlingAPrompt) {
        this.isHandlingAPrompt = true;

        let currentPrompt = this.prompt;
        this.prompt = null;

        this.messages.push({
          prompt: currentPrompt,
          response: null,
        });

        await this.getResponse(currentPrompt);
      }
    },
    async getResponse(currentPrompt) {
      let res = await fetch(
        `https://localhost:44322/chat/${this.chatId}/user/${this.userId}/${currentPrompt}`,
        {
          method: 'GET',
          headers: new Headers({
            Authorization: 'Bearer ' + localStorage.getItem('token'),
          }),
        },
      );

      if (res.ok) {
        const reader = res.body.getReader();
        const textDecoder = new TextDecoder();

        while (true) {
          const { done, value } = await reader.read();

          if (done) {
            break;
          }

          const chunkText = textDecoder.decode(value);
          await this.addToLastMessage(chunkText);
        }
        this.isHandlingAPrompt = false;

        if (this.chatId == 0) {
          this.$emit('new-chat-prompted');
        }
      }
    },
    async onDeleteChat() {
      const { success, error } = await api.deleteChat(this.chatId);

      if (success) {
        this.onChatReset();
        showNotification('Success', 'Chat deleted.', 'success', 2000);
        this.$emit('chat-deleted');
      } else {
        showNotification('Error', error, 'error', 2000);
      }
    },
    onChatReset() {
      this.messages = [];
      this.$emit('chat-reset');
    },
    async addToLastMessage(chunk) {
      if (this.messages[this.messages.length - 1].response == null) {
        this.messages[this.messages.length - 1].response = chunk;
      } else {
        this.messages[this.messages.length - 1].response += chunk;
      }
      await this.scrollChatToBottom();
    },
    async onShowLeaveFeedback() {
      this.showLeaveFeedback = false;
      await nextTick();
      this.showLeaveFeedback = true;
    },
    async scrollChatToBottom() {
      const el = this.$refs.chatMessages;

      if (el) {
        await nextTick();
        el.scrollTop = el.scrollHeight;
      }
    },
  },
  watch: {
    async chatId() {
      if (this.chatId) {
        const { data } = await api.getChatMessages(this.chatId);
        this.messages = data;
        await this.scrollChatToBottom();
      }
    },
    async messages() {
      await this.scrollChatToBottom();
    },
    async prompt() {
      await this.scrollChatToBottom();
    },
  },
  async created() {
    if (this.chatId) {
      const { data } = await api.getChatMessages(this.chatId);
      this.messages = data;
    }
  },
  async mounted() {
    await this.scrollChatToBottom();
  },
};
</script>

<style lang="scss">
.chat {
  display: flex;
  flex-direction: column;
  height: 100%;
  &__content {
    padding: 1rem;
    flex: 1;
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }
  &__inner {
    height: 100%;
    position: relative;
  }
  &__bar {
    padding-top: 1rem;
    border-top: 1px solid $border;
    display: flex;
    gap: 1rem;
  }
  &__input {
    align-items: center;
    display: flex;
    width: 100%;
    input {
      flex: 1;
    }
    i {
      position: relative;
      right: 1.75rem;
      color: #6b7280;
      font-size: 1.2em;
    }
  }
  &__messages {
    position: absolute;
    left: 0;
    top: 0;
    right: 0;
    bottom: 0;
    display: flex;
    flex-direction: column;
    flex: 1;
    gap: 1rem;
    overflow: hidden;
    overflow-x: hidden;
    overflow-y: auto;
    scroll-behavior: smooth;
  }
  &__bubble {
    padding: 0 1rem;
  }
  &__prompt,
  &__response {
    max-width: 75%;
    position: relative;
    padding: 0.625rem 0.875rem;
    border-radius: 1.5rem;
    &:before,
    &:after {
      content: '';
      position: absolute;
      height: 1.25rem;
    }
    &:before {
      bottom: -0.125rem;
      transform: translate(0, -0.125rem);
    }
    &:after {
      bottom: -0.125rem;
      transform: translate(-1.875rem, -0.125rem);
      background: $background;
    }
  }
  &__prompt {
    color: $promptBubbleText;
    background: $promptBubbleBg;
    float: right;
    &:before {
      right: -0.5rem;
      border-right: 1.25rem solid $promptBubbleBg;
      border-bottom-left-radius: 0.625rem;
    }
    &:after {
      right: -3.5rem;
      width: 1.625rem;
      border-bottom-left-radius: 0.625rem;
    }
  }

  &__response {
    color: $responseBubbleText;
    background: $responseBubbleBg;
    float: left;
    &:before {
      left: -0.5rem;
      border-left: 20px solid $responseBubbleBg;
      border-bottom-right-radius: 0.625rem;
    }
    &:after {
      left: 4px;
      width: 26px;
      border-bottom-right-radius: 0.625rem;
    }
  }
}
</style>
