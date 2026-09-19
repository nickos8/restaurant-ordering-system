<script setup>
import { computed } from 'vue'

const props = defineProps({
  items: {
    type: Array,
    required: true
  }
})

const emit = defineEmits(['increase-item', 'decrease-item'])

const total = computed(() =>
  props.items.reduce((sum, line) => sum + line.price * line.quantity, 0)
)
</script>

<template>
  <div class="cart">
    <h2>Cart</h2>

    <p v-if="items.length === 0">Your cart is empty. Add something from the menu.</p>

    <ul v-else>
      <li v-for="line in items" :key="line.id">
        <span class="name">{{ line.name }}</span>
        <span class="qty-controls">
          <button @click="emit('decrease-item', line.id)">-</button>
          <span class="qty">{{ line.quantity }}</span>
          <button @click="emit('increase-item', line.id)">+</button>
        </span>
        <span class="line-total">₱{{ (line.price * line.quantity).toFixed(2) }}</span>
      </li>
    </ul>

    <div v-if="items.length > 0" class="total-row">
      <span>Total</span>
      <span>₱{{ total.toFixed(2) }}</span>
    </div>
  </div>
</template>

<style scoped>
.cart { max-width: 480px; margin: 20px auto; font-family: sans-serif; }
ul { list-style: none; padding: 0; }
li { display: flex; align-items: center; gap: 10px; padding: 10px 0; border-bottom: 1px solid #ddd; }
.name { flex: 1; }
.qty-controls { display: flex; align-items: center; gap: 6px; }
.qty-controls button { width: 24px; cursor: pointer; }
.line-total { font-weight: 600; min-width: 70px; text-align: right; }
.total-row { display: flex; justify-content: space-between; font-weight: 700; padding-top: 10px; }
</style>