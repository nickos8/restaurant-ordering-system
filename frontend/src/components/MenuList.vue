<script setup>
import { ref, onMounted } from 'vue'

const menuItems = ref([])
const isLoading = ref(true)
const errorMessage = ref('')

const emit = defineEmits(['add-to-cart'])

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
        <button @click="emit('add-to-cart', item)">Add to Cart</button>
      </li>
    </ul>
  </div>
</template>

<style scoped>
.menu-list { margin: 0 auto 20px; }
.menu-list h2 {
  font-size: 1.1rem;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.05em;
  margin: 0 0 10px;
}
ul { list-style: none; padding: 0; margin: 0; display: flex; flex-direction: column; gap: 8px; }
li {
  display: flex;
  align-items: center;
  gap: 10px;
  padding: 12px 14px;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
}
.name { flex: 1; font-weight: 600; }
.category { color: var(--color-text-muted); font-size: 0.8rem; }
.price { font-weight: 600; min-width: 64px; text-align: right; }
button { padding: 6px 12px; cursor: pointer; }
</style>