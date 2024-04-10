import axios from 'axios';

export default {
  async validateUser() {
    try {
      const res = await axios.get('user/validate');
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async getUser(userId) {
    try {
      const res = await axios.get(`user/${userId}`);
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async deleteUser(userId) {
    try {
      const res = await axios.delete(`user/${userId}`);
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async getRoles() {
    try {
      const res = await axios.get('role');
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async getChats(userId) {
    try {
      const res = await axios.get(`chat/user/${userId}`);
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async deleteChat(id) {
    try {
      const res = await axios.delete(`chat/${id}`);
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async getChatMessages(chatId) {
    try {
      const res = await axios.get(`chat/${chatId}/messages`);
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async getDocuments(userId) {
    try {
      const res = await axios.get(`document?userId=${userId}`);
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async deleteDocument(documentId) {
    try {
      const res = await axios.delete(`document/${documentId}`);
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async uploadDocument(document, userId) {
    const formData = new FormData();
    formData.append('file', document);
    formData.append('userId', userId);

    try {
      const res = await axios.post('document', formData, {
        headers: {
          'Content-Type': 'multipart/form-data',
        },
      });
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async postFeedback(userId, message) {
    try {
      const res = await axios.post('feedback', { userId, message });
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async getCustomInstruction(userId) {
    try {
      const res = await axios.get(`user/${userId}`);
      return { success: res.status === 200, data: res.data.customInstruction };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async updateCustomInstruction(userId, customInstruction) {
    try {
      const res = await axios.put(
        `user/${userId}/customInstruction/${customInstruction}`,
      );
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async getUsers() {
    try {
      const res = await axios.get('user');
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async updateUser(userId, newRoleId) {
    try {
      const res = await axios.put(`user/${userId}/role/${newRoleId}`);
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async login(username, password) {
    try {
      const res = await axios.post('user/login', { username, password });
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async register(username, password) {
    try {
      const res = await axios.post('user/register', { username, password });
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      console.log(error);
      console.log(error.response.data);
      return { success: false, error: error.response.data };
    }
  },
  async getFeedbakcs() {
    try {
      const res = await axios.get('feedback');
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async getArchivedFeedbacks() {
    try {
      const res = await axios.get('feedback/archived');
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
  async archiveFeedback(feedbackId) {
    try {
      const res = await axios.put(`feedback/${feedbackId}/archive`);
      return { success: res.status === 200, data: res.data };
    } catch (error) {
      return { success: false, error: error.response.data };
    }
  },
};
