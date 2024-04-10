<template>
  <Dialog
    modal
    closeOnEscape
    v-model:visible="showModal"
    header="LEAVE FEEDBACK"
    :closable="true"
    :draggable="true"
    :style="{ width: '600px' }"
  >
    <div class="feedback-editor">
      <label>How's your experience with the app?</label>
      <span class="feedback-editor__content">
        <TextArea
          v-model="userFeedback"
          autoResize
          rows="10"
          cols="60"
        ></TextArea>
      </span>
      <Button
        label="Submit"
        :disabled="isSubmitDisabled"
        @click="submitUserFeedback"
      />
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
    visible: { name: 'visible', required: true },
  },
  components: {
    Button,
    InputText,
    TextArea,
    Dialog,
  },
  data() {
    return {
      userFeedback: '',
      showModal: false,
      isSubmitDisabled: true,
    };
  },
  methods: {
    async submitUserFeedback() {
      if (!!this.userFeedback) {
        this.isSubmitDisabled = true;
        const { success, error } = await api.postFeedback(
          this.userId,
          this.userFeedback,
        );

        if (success) {
          this.showModal = false;
          this.userFeedback = '';
          showNotification(
            'Success',
            'User feedback submitted.',
            'success',
            2000,
          );
        } else {
          showNotification('Error', error, 'error', 2000);
        }
        this.isSubmitDisabled = false;
      } else {
        showNotification(
          'Error',
          'Please fill in feedback first.',
          'error',
          2000,
        );
      }
    },
  },
  watch: {
    visible() {
      if (this.visible) {
        this.showModal = true;
      }
    },
    userFeedback() {
      if (this.userFeedback == null || this.userFeedback == '') {
        this.isSubmitDisabled = true;
      } else {
        this.isSubmitDisabled = false;
      }
    },
  },
};
</script>

<style lang="scss">
.feedback-editor {
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
