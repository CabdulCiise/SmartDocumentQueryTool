<template>
  <RouterView />
  <Toast position="bottom-right" />
</template>

<script>
import { RouterView } from 'vue-router';
import Toast from 'primevue/toast';

export default {
  props: {},
  components: {
    RouterView,
    Toast,
  },
};
</script>
