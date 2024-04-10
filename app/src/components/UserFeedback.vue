<template>
  <div class="user-feedback">
    <h3 class="user-feedback__heading">Review User Feedback</h3>
    <TabView>
      <TabPanel header="New">
        <div class="user-feedback__panel">
          <div class="user-feedback__content">
            <div class="user-feedback__wrapper">
              <DataTable
                scrollable
                scroll-height="flex"
                :value="feedbacks"
                paginator
                :rows="20"
                :rowsPerPageOptions="[5, 10, 20, 50]"
                paginatorTemplate="RowsPerPageDropdown FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink"
                currentPageReportTemplate="{first} to {last} of {totalRecords}"
              >
                <Column
                  field="createdAt"
                  sortable
                  header="Date"
                  style="width: 15%"
                >
                  <template #body="{ data }">{{
                    $filters.date(data.createdAt)
                  }}</template>
                </Column>
                <Column
                  field="message"
                  sortable
                  header="Feedback"
                  style="width: 75%"
                ></Column>
                <Column field="" header="" style="width: 10%">
                  <template #body="slotProps">
                    <Button
                      label="Archive"
                      @click="archiveFeedback(slotProps.data.id)"
                    />
                  </template>
                </Column>
              </DataTable>
            </div>
          </div>
        </div>
      </TabPanel>
      <TabPanel header="Archived">
        <div class="user-feedback__panel">
          <div class="user-feedback__content">
            <div class="user-feedback__wrapper">
              <DataTable
                scrollable
                scroll-height="flex"
                :value="archivedFeedbacks"
                paginator
                :rows="20"
                :rowsPerPageOptions="[5, 10, 20, 50]"
                paginatorTemplate="RowsPerPageDropdown FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink"
                currentPageReportTemplate="{first} to {last} of {totalRecords}"
              >
                <Column
                  field="createdAt"
                  sortable
                  header="Date"
                  style="width: 15%"
                >
                  <template #body="{ data }">{{
                    $filters.date(data.createdAt)
                  }}</template>
                </Column>
                <Column
                  field="message"
                  sortable
                  header="Feedback"
                  style="width: 85%"
                ></Column>
              </DataTable>
            </div>
          </div>
        </div>
      </TabPanel>
    </TabView>
  </div>
</template>

<script>
import api from '../api/api';

import Dialog from 'primevue/dialog';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import TabView from 'primevue/tabview';
import TabPanel from 'primevue/tabpanel';

import { showNotification } from '../services/notification';

export default {
  props: {},
  components: {
    Dialog,
    DataTable,
    Column,
    Button,
    TabView,
    TabPanel,
  },
  data() {
    return {
      feedbacks: [],
      archivedFeedbacks: [],
    };
  },
  methods: {
    async archiveFeedback(feebackId) {
      const { success } = await api.archiveFeedback(feebackId);

      if (success) {
        await this.setFeedbackData();

        showNotification('Success', 'User feedback archived.', 'success', 2000);
      } else {
        showNotification(
          'Error',
          'Failed to archive user feedback.',
          'error',
          2000,
        );
      }
    },
    async setFeedbackData() {
      const [feedbackRes, archivedFeedbackRes] = await Promise.all([
        api.getFeedbakcs(),
        api.getArchivedFeedbacks(),
      ]);

      this.feedbacks = feedbackRes.data;
      this.archivedFeedbacks = archivedFeedbackRes.data;
    },
  },
  async created() {
    await this.setFeedbackData();
  },
};
</script>

<style lang="scss">
.user-feedback {
  padding: 1rem;
  height: 100%;
  display: flex;
  flex-direction: column;
  gap: 1rem;
  &__heading {
    margin: 0;
  }

  .p-tabview {
    height: 100%;
    display: flex;
    flex-direction: column;

    .p-tabview-panels {
      flex: 1;
    }

    .p-tabview-panel {
      height: 100%;
    }
  }

  &__panel {
    height: 100%;
  }

  &__content {
    position: relative;
    height: 100%;
  }

  &__wrapper {
    position: absolute;
    top: 0;
    left: 0;
    right: 0;
    bottom: 0;
  }

  &__heading {
    margin-top: 0;
  }
}
</style>
