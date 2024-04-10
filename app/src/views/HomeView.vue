<template>
  <Login @logged-in="onLoggedIn" v-if="!userId" />

  <div v-else class="home">
    <div class="home__sidebar">
      <Button
        icon="pi pi-comments"
        v-tooltip="'Chat'"
        @click="showChat = true"
      />
      <Button
        icon="pi pi-file-edit"
        v-tooltip="'Document Editor'"
        @click="showDocumentEditor = true"
      />
      <Button
        icon="pi pi-users"
        v-tooltip="'System Users'"
        v-if="isAdmin"
        @click="showUsers = true"
      />
      <Button
        icon="pi pi-check"
        v-tooltip="'Review feedback'"
        v-if="isAdmin"
        @click="showUserFeedback = true"
      />
    </div>
    <div class="home__content">
      <div class="home__header">
        <label v-if="!!chatName && showChat" class="home__welcome">{{
          chatName
        }}</label>
        <label v-else class="home__welcome">Document Query Tool</label>
        <div class="home__profile">
          <label class="home__user">Hi there, {{ username }}</label>
          <Button
            icon="pi pi-user"
            text
            type="button"
            @click="toggleMenu"
            aria-haspopup="true"
            aria-controls="overlay_menu"
          />
          <Menu ref="menu" id="overlay_menu" :model="items" :popup="true" />
        </div>
      </div>
      <div class="home__body">
        <ProfileEditor
          v-if="showProfileEditor"
          :user-id="userId"
          :custom-instruction="customInstruction"
          @custom-instruction-updated="onCustomInstructionUpdated"
        />
        <User v-if="showUsers" />
        <UserFeedback v-if="showUserFeedback" />
        <DocumentEditor v-if="showDocumentEditor" :user-id="userId" />
        <Chat
          v-if="showChat"
          :user-id="userId"
          :chat-id="lastChatId"
          @chat-reset="onChatReset"
          @chat-deleted="onChatDeleted"
          @new-chat-prompted="onNewChatPrompted"
        />
      </div>
    </div>
    <div class="home__chat-history" ref="chatHistory" v-if="showChat">
      <label>Chat History</label>
      <Button
        text
        type="button"
        v-for="chat in chats"
        @click="selectedChat = chat"
        >{{ chat.name }}</Button
      >
    </div>
  </div>
</template>

<script>
import api from '../api/api';

import Login from '../components/Login.vue';
import Chat from '../components/Chat.vue';
import ProfileEditor from '../components/ProfileEditor.vue';
import UserFeedback from '../components/UserFeedback.vue';
import User from '../components/User.vue';
import DocumentEditor from '../components/DocumentEditor.vue';
import Button from 'primevue/button';
import Menu from 'primevue/menu';
import { PrimeIcons } from 'primevue/api';
import { nextTick } from 'vue';
import { jwtDecode } from 'jwt-decode';

export default {
  props: {},
  components: {
    Login,
    Chat,
    ProfileEditor,
    UserFeedback,
    User,
    DocumentEditor,
    Button,
    Menu,
  },
  data() {
    return {
      userId: null,
      isAdmin: null,
      username: null,
      customInstruction: null,
      selectedChat: null,
      showProfileEditor: false,
      showUserFeedback: false,
      showDocumentEditor: false,
      showChat: false,
      showUsers: false,
      adminRoleId: null,
      chats: [],
      lastChatId: 0,
      chatName: '',
      items: [
        {
          label: 'Options',
          items: [
            {
              label: 'Profile',
              icon: PrimeIcons.USER_EDIT,
              command: () => this.onShowProfileEditor(),
            },
            {
              label: 'Logout',
              icon: PrimeIcons.SIGN_OUT,
              command: () => this.onLoggedOut(),
            },
          ],
        },
      ],
    };
  },
  async created() {
    await this.validateUser();
  },
  methods: {
    async onLoggedIn() {
      this.hideAll();
      await this.setUserData();
    },
    onLoggedOut() {
      localStorage.removeItem('token');
      this.hideAll();
      this.resetUserData();
    },
    async onShowProfileEditor() {
      this.showProfileEditor = false;
      await nextTick();
      this.showProfileEditor = true;
    },
    hideAll() {
      this.showChat = false;
      this.showDocumentEditor = false;
      this.showUserFeedback = false;
    },
    toggleMenu(event) {
      this.$refs.menu.toggle(event);
    },
    async getChats() {
      const { data } = await api.getChats(this.userId);
      this.chats = data;
    },
    onCustomInstructionUpdated(customInstruction) {
      this.customInstruction = customInstruction;
    },
    async onChatDeleted() {
      await this.getChats();
      await nextTick();
      await nextTick();
      this.selectedChat = null;
    },
    async onChatReset() {
      this.selectedChat = null;
    },
    async validateUser() {
      const { success } = await api.validateUser();

      if (success) {
        await this.setUserData();
      } else {
        this.resetUserData();
      }

      return success;
    },
    resetUserData() {
      this.userId = null;
      this.username = null;
      this.isAdmin = null;
      this.customInstruction = null;
    },
    async setUserData() {
      const token = localStorage.getItem('token');
      const decodedToken = jwtDecode(token, { complete: true });

      this.userId = decodedToken.userId;
      this.username = decodedToken.username;
      this.isAdmin = decodedToken.isAdmin.toLowerCase() === 'true';

      const { data } = await api.getCustomInstruction(this.userId);
      this.customInstruction = data;
    },
    async onNewChatPrompted() {
      await this.getChats();
      this.scollChatHistoryToTop();
    },
    scollChatHistoryToTop() {
      const el = this.$refs.chatHistory;

      if (el) {
        el.scrollTop = 0;
      }
    },
  },
  watch: {
    async userId() {
      if (!!this.userId) {
        await this.getChats();

        this.selectedChat = !!this.chats ? this.chats[0] : null;
        this.showChat = true;
      }
    },
    showDocumentEditor() {
      if (this.showDocumentEditor) {
        this.showUserFeedback = false;
        this.showChat = false;
        this.showUsers = false;
      }
    },
    showChat() {
      if (this.showChat) {
        this.showUserFeedback = false;
        this.showDocumentEditor = false;
        this.showUsers = false;
        this.selectedChat = !!this.chats ? this.chats[0] : null;
      }
    },
    chats() {
      this.selectedChat = !!this.chats ? this.chats[0] : null;
    },
    showUsers() {
      if (this.showUsers) {
        this.showUserFeedback = false;
        this.showDocumentEditor = false;
        this.showChat = false;
      }
    },
    showUserFeedback() {
      if (this.showUserFeedback) {
        this.showUsers = false;
        this.showDocumentEditor = false;
        this.showChat = false;
      }
    },
    selectedChat() {
      if (!!this.selectedChat) {
        this.showChat = true;
        this.chatName = this.selectedChat.name;
        this.lastChatId = this.selectedChat.id;
      } else {
        this.chatName = '';
        this.lastChatId = 0;
      }
    },
  },
};
</script>

<style lang="scss">
.home {
  display: flex;
  height: 100vh;

  &__sidebar {
    display: flex;
    flex-direction: column;
    border-right: 1px solid $border;
    background: $sidebar;
    justify-content: end;
    align-items: right;
    padding: 0.5rem;
    gap: 0.5rem;
  }

  &__chat-history {
    border-left: 1px solid $border;
    padding: 0.5rem;
    margin-top: 1rem;
    gap: 0.5rem;
    display: flex;
    flex-direction: column;
    justify-content: start;
    width: 15%;
    min-width: 200px;
    overflow: auto;
    scroll-behavior: smooth;

    Button {
      white-space: nowrap;
      text-overflow: ellipsis;
      overflow: hidden;
      min-height: 30px;
      font-size: 15px;
      padding: 0.5rem;
      :hover {
        background: gray;
      }
    }
  }

  &__content {
    flex: 1;
    display: flex;
    flex-direction: column;
  }

  &__header {
    padding: 1rem;
    border-bottom: 1px solid $border;
    display: flex;
    width: 100%;
    justify-content: space-between;
  }
  &__welcome {
    flex: 1;
    line-height: 0;
    margin: auto 0;
  }
  &__profile {
    display: flex;
    gap: 1rem;
  }
  &__user {
    line-height: 0;
    margin: auto 0;
  }

  &__body {
    flex: 1;
    overflow: hidden;
  }
}
</style>
