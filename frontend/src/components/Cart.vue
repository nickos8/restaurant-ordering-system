<script setup>
import { computed } from 'vue'

const props = defineProps({
  items: {
    type: Array,
    required: true
  },
  isCheckingOut: {
    type: Boolean,
    default: false
  }
})

const emit = defineEmits(['increase-item', 'decrease-item', 'checkout'])

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

    <div v-if="items.length > 0" class="checkout-row">
      <button :disabled="isCheckingOut" @click="emit('checkout', 'Cash')">
        {{ isCheckingOut ? 'Placing order...' : 'Pay with Cash' }}
      </button>
      <button :disabled="isCheckingOut" @click="emit('checkout', 'Card')">
        {{ isCheckingOut ? 'Placing order...' : 'Pay with Card' }}
      </button>
    </div>
  </div>
</template>

<style scoped>
.cart {
  margin: 0 auto;
  background: var(--color-surface);
  border: 1px solid var(--color-border);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
  padding: 16px 18px;
}
.cart h2 {
  font-size: 1.1rem;
  color: var(--color-text-muted);
  text-transform: uppercase;
  letter-spacing: 0.05em;
  margin: 0 0 10px;
}
.cart p { color: var(--color-text-muted); }
ul { list-style: none; padding: 0; margin: 0; }
li { display: flex; align-items: center; gap: 10px; padding: 10px 0; border-bottom: 1px solid var(--color-border); }
.name { flex: 1; font-weight: 600; }
.qty-controls { display: flex; align-items: center; gap: 8px; }
.qty-controls button { width: 28px; height: 28px; cursor: pointer; padding: 0; }
.qty { min-width: 16px; text-align: center; }
.line-total { font-weight: 600; min-width: 70px; text-align: right; }
.total-row { display: flex; justify-content: space-between; font-weight: 700; padding-top: 14px; font-size: 1.05rem; }
.checkout-row { display: flex; gap: 10px; margin-top: 16px; }
.checkout-row button { flex: 1; padding: 12px; cursor: pointer; }
</style>