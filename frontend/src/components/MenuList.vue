<script setup>
import { ref, onMounted } from 'vue'

const menuItems = ref([])
const isLoading = ref(true)
const errorMessage = ref('')

onMounted(async () => {
  try {
    const response = await fetch('http://localhost:5281/api/menuitems')
    if (!response.ok) throw new Error('Request failed with status ' + response.status)
    menuItems.value = await response.json()
  } catch (err) {
    errorMessage.value = 'Could not load the menu. Is the API running?'
  } finally {
    isLoading.value = false
  }
})
</script>

<template>
  <div class="menu-list">
    <h1>Menu</h1>

    <p v-if="isLoading">Loading menu...</p>
    <p v-else-if="errorMessage">{{ errorMessage }}</p>

    <ul v-else>
      <li v-for="item in menuItems" :key="item.id">
        <span class="name">{{ item.name }}</span>
        <span class="category">{{ item.category }}</span>
        <span class="price">₱{{ item.price.toFixed(2) }}</span>
      </li>
    </ul>
  </div>
</template>

<style scoped>
.menu-list { max-width: 480px; margin: 40px auto; font-family: sans-serif; }
ul { list-style: none; padding: 0; }
li { display: flex; justify-content: space-between; padding: 10px 0; border-bottom: 1px solid #ddd; }
.category { color: #888; font-size: .85rem; }
.price { font-weight: 600; }
</style>