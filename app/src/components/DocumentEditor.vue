<template>
  <div class="document-editor">
    <div class="document-editor__panel">
      <h3 class="document-editor__heading">Upload New Documents</h3>
      <div class="document-editor__content">
        <div class="document-editor__wrapper">
          <FileUpload
            @select="onFilesUpload"
            :auto="true"
            accept=".pdf, .txt, .docx"
            :showCancelButton="false"
            :showUploadButton="false"
            :maxFileSize="10000000"
          >
          </FileUpload>
        </div>
      </div>
    </div>
    <div class="document-editor__panel">
      <h3 class="document-editor__heading">Delete Documents</h3>
      <div class="document-editor__content">
        <DataTable
          :value="documents"
          paginator
          scrollable
          scroll-height="flex"
          :rows="5"
          :rowsPerPageOptions="[5, 10, 20, 50]"
        >
          <Column field="created_date" sortable header="Date" style="width: 8%">
            <template #body="{ data }">{{
              $filters.date(data.uploadedAt)
            }}</template>
          </Column>
          <Column
            field="name"
            sortable
            header="File Name"
            style="width: 40%"
          ></Column>
          <Column field="cost" sortable header="Cost" style="width: 8%">
            <template #body="{ data }">{{
              $filters.currency(data.cost.toFixed(8))
            }}</template>
          </Column>
          <Column
            field="processTime"
            sortable
            header="Process Time"
            style="width: 8%"
          ></Column>
          <Column field="" header="" style="width: 5%">
            <template #body="{ data }">
              <Button
                icon="pi pi-trash"
                type="submit"
                @click="deleteDocument(data.id, data.name)"
                severity="danger"
                label=""
              />
            </template>
          </Column>
        </DataTable>
      </div>
    </div>
  </div>
</template>

<script>
import api from '../api/api';

import FileUpload from 'primevue/fileupload';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import Badge from 'primevue/badge';
import ProgressBar from 'primevue/progressbar';

import { showNotification } from '../services/notification';

export default {
  props: {
    userId: { name: 'useId', required: true },
  },
  components: {
    FileUpload,
    DataTable,
    Column,
    Button,
    Badge,
    ProgressBar,
  },
  data() {
    return {
      documents: [],
    };
  },
  methods: {
    async uploadDocument(document) {
      const { success, error } = await api.uploadDocument(
        document,
        this.userId,
      );

      if (success) {
        const { data } = await api.getDocuments(this.userId);
        this.documents = data;
        showNotification(
          'Success',
          'File added successfully.',
          'success',
          2000,
        );
      } else {
        showNotification('Error', error, 'error', 2000);
      }
    },
    async deleteDocument(uploadedDocId) {
      const { success, error } = await api.deleteDocument(uploadedDocId);

      if (success) {
        showNotification(
          'Success',
          'File deleted successfully.',
          'success',
          2000,
        );
        const { data } = await api.getDocuments(this.userId);
        this.documents = data;
      } else {
        showNotification('Error', error, 'error', 2000);
        return;
      }
    },
    onFilesUpload(event) {
      event.files.forEach((file) => {
        this.uploadDocument(file);
      });
    },
  },
  async mounted() {
    const { data } = await api.getDocuments(this.userId);
    this.documents = data;
  },
};
</script>
<style lang="scss">
.document-editor {
  padding: 1rem;
  display: flex;
  flex-direction: column;
  gap: 1rem;
  height: 100%;
  &__heading {
    margin: 0;
  }
  &__panel {
    display: flex;
    flex-direction: column;
    gap: 1rem;
    flex: 1;
  }
  &__content {
    flex: 1;
    position: relative;
  }
  &__wrapper {
    position: absolute;
    left: 0;
    top: 0;
    right: 0;
    bottom: 0;
    overflow-y: auto;
  }
  .p-file-upload {
    height: 100%;
    display: flex;
    flex-direction: column;
  }
  .p-fileupload-file {
    margin: 0;
    padding: 0.5rem;
    height: 5rem;
  }
  .p-fileupload-file-thumbnail {
    // TODO Remove to display icon (broken atm)
    display: none;
  }
  .p-fileupload-buttonbar {
    padding: 0.5rem;
  }
  .p-fileupload-content {
    flex: 1;
    display: grid;
    grid-template-columns: repeat(2, 1fr);
    grid-template-rows: 5rem;
    gap: 0.5rem;
    padding: 1rem 0.5rem;
  }
  .p-fileupload-file-details {
    flex: 1;
  }
  .p-fileupload-file-remove {
    color: red;
  }
}
</style>
