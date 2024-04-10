import { toast } from './primevue';

function showNotification(
  title = 'Title Here',
  message = 'Message Here',
  severity = 'info', // success, info, warn, error
  duration = 2000,
) {
  toast.add({
    severity,
    summary: title,
    detail: message,
    life: duration,
  });
}

export { showNotification };
