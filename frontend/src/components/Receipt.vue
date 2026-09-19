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
.receipt { max-width: 480px; margin: 40px auto; font-family: sans-serif; }
.order-id { font-weight: 700; }
ul { list-style: none; padding: 0; }
li { display: flex; justify-content: space-between; padding: 8px 0; border-bottom: 1px solid #ddd; }
.total-row { display: flex; justify-content: space-between; font-weight: 700; padding-top: 10px; }
button { margin-top: 20px; padding: 10px; width: 100%; cursor: pointer; }
</style>