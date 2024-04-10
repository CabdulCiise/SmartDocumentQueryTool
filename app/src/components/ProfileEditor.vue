<template>
  <Dialog
    modal
    v-model:visible="showModal"
    header="USER PROFILE"
    :closable="true"
    :draggable="true"
    :style="{ width: '600px' }"
  >
    <div class="profile-editor">
      <label>These instructions are fed into AI model</label>
      <span class="profile-editor__content">
        <TextArea
          v-model="newCustomInstruction"
          autoResize
          rows="10"
          cols="60"
        ></TextArea>
      </span>
      <Button label="Save Changes" @click="updateCustomInstruction" />
    </div>
  </Dialog>
</template>

<script>
import api from '../api/api';

import Button from 'primevue/button';
import InputText from 'primevue/inputtext';
import Dialog from 'primevue/dialog';
import TextArea from 'primevue/textarea';

import { showNotification } from '../services/notification';

export default {
  props: {
    userId: { name: 'userId', required: true },
    customInstruction: { name: 'customInstruction', required: true },
  },
  emits: ['custom-instruction-updated'],
  components: {
    Button,
    InputText,
    TextArea,
    Dialog,
  },
  data() {
    return {
      newCustomInstruction: '',
      showModal: true,
    };
  },
  methods: {
    async updateCustomInstruction() {
      if (this.newCustomInstruction === '') {
        this.newCustomInstruction = null;
      }

      const { success, data, error } = await api.updateCustomInstruction(
        this.userId,
        this.newCustomInstruction,
      );

      if (success) {
        this.showModal = false;

        this.$emit('custom-instruction-updated', this.newCustomInstruction);

        showNotification('Success', 'User Profile Updated.', 'success', 2000);
      } else {
        showNotification('Error', error, 'error', 2000);
      }
    },
  },
  mounted() {
    this.newCustomInstruction = this.customInstruction;
  },
};
</script>

<style lang="scss">
.profile-editor {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
  &__header {
  }
  &__content {
    textarea {
      width: 100%;
    }
  }
}
</style>
