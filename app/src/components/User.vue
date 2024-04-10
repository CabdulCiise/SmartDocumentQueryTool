<template>
  <div class="user-page">
    <h3 class="user-page__heading">System Users</h3>
    <div class="user-page__content">
      <div class="user-page__wrapper">
        <DataTable
          scrollable
          scroll-height="flex"
          :value="users"
          paginator
          :rows="10"
          :rowsPerPageOptions="[5, 10, 20, 50]"
          paginatorTemplate="RowsPerPageDropdown FirstPageLink PrevPageLink CurrentPageReport NextPageLink LastPageLink"
          currentPageReportTemplate="{first} to {last} of {totalRecords}"
        >
          <Column
            field="username"
            sortable
            header="Username"
            style="width: 10%"
          ></Column>
          <Column
            field="createdAt"
            sortable
            header="Created Date"
            style="width: 15%"
          >
            <template #body="{ data }">{{
              $filters.date(data.createdAt)
            }}</template>
          </Column>
          <Column field="roleId" header="Role" style="width: 10%">
            <template #body="slotProps">
              <Dropdown
                v-model="slotProps.data.roleId"
                :options="roles"
                optionLabel="name"
                optionValue="id"
                placeholder="Select User Role"
                @change="
                  onRoleUpdated(slotProps.data.id, slotProps.data.roleId)
                "
              />
            </template>
          </Column>
          <Column
            field="numberOfDocuments"
            sortable
            header="Uploaded Docs"
            style="width: 13%"
          ></Column>
          <Column
            field="lastUploadedDate"
            sortable
            header="Last Upload Date"
            style="width: 15%"
          >
            <template #body="{ data }">{{
              $filters.date(data.lastUploadedDate)
            }}</template>
          </Column>
          <Column
            field="totalUploadCost"
            sortable
            header="Total Upload Cost"
            style="width: 15%"
          >
            <template #body="{ data }">{{
              $filters.currency(data.totalUploadCost.toFixed(8))
            }}</template>
          </Column>
          <Column
            field="numberOfChats"
            sortable
            header="Chats"
            style="width: 10%"
          ></Column>
          <Column
            field="lastChatDate"
            sortable
            header="Last Chat Date"
            style="width: 15%"
          >
            <template #body="{ data }">{{
              $filters.date(data.lastChatDate)
            }}</template>
          </Column>
          <Column field="" header="" style="width: 10%">
            <template #body="slotProps">
              <Button
                label=""
                icon="pi pi-trash"
                severity="danger"
                @click="onDeleteUser(slotProps.data.id)"
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

import Dropdown from 'primevue/dropdown';
import DataTable from 'primevue/datatable';
import Column from 'primevue/column';
import Button from 'primevue/button';
import { showNotification } from '../services/notification';

export default {
  components: {
    Dropdown,
    DataTable,
    Column,
    Button,
  },
  data() {
    return {
      users: [],
      roles: [],
    };
  },
  methods: {
    async onRoleUpdated(userId, newRoleId) {
      await api.updateUser(userId, newRoleId);

      const { data } = await api.getUsers();
      this.users = data;
    },
    async onDeleteUser(userId) {
      const { success, error } = await api.deleteUser(userId);

      if (success) {
        const { data } = await api.getUsers();
        this.users = data;
        showNotification('Success', 'User deleted.', 'success', 2000);
      } else {
        showNotification('Error', error, 'error', 2000);
      }
    },
  },
  async created() {
    const [usersRes, rolesRes] = await Promise.all([
      api.getUsers(),
      api.getRoles(),
    ]);
    this.users = usersRes.data;
    this.roles = rolesRes.data;
  },
};
</script>

<style lang="scss">
.user-page {
  padding: 1rem;
  height: 100%;
  display: flex;
  flex-direction: column;
  gap: 1rem;
  &__heading {
    margin: 0;
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
