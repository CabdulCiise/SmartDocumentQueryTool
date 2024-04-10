<template>
  <Dialog
    modal
    v-model:visible="loginModalVisible"
    header="LOG IN"
    :closable="false"
    :draggable="true"
  >
    <div class="login-dialog">
      <form @submit.prevent="submitLogin" class="login-dialog__form">
        <InputText
          type="input"
          class="login-dialog__input"
          placeholder="Enter Username"
          required
          autofocus
          v-model="username"
        ></InputText>
        <InputText
          type="password"
          class="login-dialog__input"
          placeholder="Password"
          required
          v-model="password"
        ></InputText>
        <Button
          type="submit"
          raised
          class="login-dialog__submit"
          label="Sign In"
        ></Button>
        <div class="login-dialog__register">
          Don't have an account?
          <a href="#" @click.prevent="onRegistering">Register</a>
        </div>
      </form>
    </div>
  </Dialog>
  <Dialog
    modal
    v-model:visible="registerModalVisible"
    header="CREATE AN ACCOUNT"
    :closable="false"
    :draggable="true"
  >
    <div class="register-dialog">
      <form @submit.prevent="register" class="register-dialog__form">
        <InputText
          type="input"
          class="register-dialog__input"
          placeholder="Enter Username"
          required
          autofocus
          v-model="username"
        ></InputText>
        <InputText
          type="password"
          class="register-dialog__input"
          placeholder="Password"
          required
          v-model="password"
        ></InputText>
        <InputText
          type="password"
          class="register-dialog__input"
          placeholder="Repeat your password"
          required
          v-model="passwordRepeat"
        ></InputText>
        <Button type="submit" class="register-dialog__submit">Register</Button>
      </form>
    </div>
  </Dialog>
</template>

<script>
import api from '../api/api';

import Dialog from 'primevue/dialog';
import InputText from 'primevue/inputtext';
import Button from 'primevue/button';

import { showNotification } from '@/services/notification';

export default {
  props: {},
  emits: ['logged-in'],
  components: {
    Dialog,
    InputText,
    Button,
  },
  data() {
    return {
      loginModalVisible: true,
      registerModalVisible: false,
      username: '',
      password: '',
      passwordRepeat: '',
      userId: null,
      isRegistering: false,
    };
  },
  methods: {
    async submitLogin() {
      const { success, data, error } = await api.login(
        this.username,
        this.password,
      );
      if (success) {
        localStorage.setItem('token', data);
        this.$emit('logged-in');
      } else {
        showNotification('Error', error, 'error', 2000);
      }
    },
    async register() {
      if (this.password === this.passwordRepeat && this.password.length > 0) {
        const { success, data, error } = await api.register(
          this.username,
          this.password,
        );
        if (success) {
          localStorage.setItem('token', data);
          this.$emit('logged-in', this.userId);
        } else {
          showNotification('Error', error, 'error', 2000);
        }
      } else {
        showNotification('Error', 'Passwords do not match.', 'error', 2000);
      }
    },
    onRegistering() {
      this.registerModalVisible = true;
      this.loginModalVisible = false;
    },
  },
};
</script>

<style scoped lang="scss">
.p-dialog-content {
  width: 100%;
  height: 100%;
}
.register-dialog,
.login-dialog {
  &__form {
    width: 20rem;
    display: flex;
    flex-direction: column;
    gap: 1rem;
  }
  &__submit {
    justify-content: center;
  }
  &__register {
    border-top: 1px solid $border;
    padding-top: 1rem;
  }
}
</style>
