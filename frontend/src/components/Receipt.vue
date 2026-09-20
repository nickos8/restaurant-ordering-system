<script setup>
defineProps({
  receipt: {
    type: Object,
    required: true
  }
})

const emit = defineEmits(['start-new-order'])
</script>

<template>
  <div class="receipt">
    <h2>Receipt</h2>
    <p class="order-id">Order #{{ receipt.orderId }}</p>
    <p>Customer: {{ receipt.customerName }}</p>
    <p>Paid via {{ receipt.paymentMethod }} on {{ new Date(receipt.paidAt).toLocaleString() }}</p>

    <ul>
      <li v-for="item in receipt.items" :key="item.id">
        <span class="name">{{ item.menuItemName }} x{{ item.quantity }}</span>
        <span class="line-total">₱{{ item.lineTotal.toFixed(2) }}</span>
      </li>
    </ul>

    <div class="total-row">
      <span>Total Paid</span>
      <span>₱{{ receipt.total.toFixed(2) }}</span>
    </div>

    <button @click="emit('start-new-order')">Start a new order</button>
  </div>
</template>

<style scoped>
.receipt {
  margin: 30px auto 0;
  background: var(--color-surface);
  border: 1px dashed var(--color-border);
  border-radius: var(--radius);
  box-shadow: var(--shadow);
  padding: 20px;
  text-align: center;
}
.receipt h2 {
  margin: 0 0 4px;
  color: var(--color-accent-dark);
}
.order-id { font-weight: 700; margin: 0 0 12px; }
.receipt p { margin: 4px 0; color: var(--color-text-muted); font-size: 0.9rem; }
ul { list-style: none; padding: 0; margin: 16px 0; text-align: left; }
li { display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px dashed var(--color-border); }
.total-row { display: flex; justify-content: space-between; font-weight: 700; padding-top: 10px; font-size: 1.1rem; }
button { margin-top: 20px; padding: 12px; width: 100%; cursor: pointer; }
</style>