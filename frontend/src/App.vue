<script setup>
import { ref } from 'vue'
import MenuList from './components/MenuList.vue'
import Cart from './components/Cart.vue'
import Receipt from './components/Receipt.vue'

const API_BASE = 'http://localhost:5281/api'

const cart = ref([])
const receipt = ref(null)
const isCheckingOut = ref(false)
const checkoutError = ref('')

function handleAddToCart(menuItem) {
  const existingLine = cart.value.find((line) => line.id === menuItem.id)
  if (existingLine) {
    existingLine.quantity++
  } else {
    cart.value.push({
      id: menuItem.id,
      name: menuItem.name,
      price: menuItem.price,
      quantity: 1
    })
  }
}

function handleIncreaseItem(menuItemId) {
  const line = cart.value.find((line) => line.id === menuItemId)
  if (line) line.quantity++
}

function handleDecreaseItem(menuItemId) {
  const line = cart.value.find((line) => line.id === menuItemId)
  if (!line) return
  if (line.quantity <= 1) {
    cart.value = cart.value.filter((l) => l.id !== menuItemId)
  } else {
    line.quantity--
  }
}

async function handleCheckout(paymentMethod) {
  isCheckingOut.value = true
  checkoutError.value = ''

  try {
    const createResponse = await fetch(`${API_BASE}/orders`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ customerName: 'Walk-in Customer' })
    })
    if (!createResponse.ok) throw new Error('Could not create the order.')
    const order = await createResponse.json()

    for (const line of cart.value) {
      const itemResponse = await fetch(`${API_BASE}/orders/${order.id}/items`, {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ menuItemId: line.id, quantity: line.quantity })
      })
      if (!itemResponse.ok) throw new Error(`Could not add "${line.name}" to the order.`)
    }

    const payResponse = await fetch(`${API_BASE}/orders/${order.id}/pay`, {
      method: 'POST',
      headers: { 'Content-Type': 'application/json' },
      body: JSON.stringify({ paymentMethod })
    })
    if (!payResponse.ok) throw new Error('Payment failed.')

    receipt.value = await payResponse.json()
    cart.value = []
  } catch (err) {
    checkoutError.value = err.message || 'Something went wrong placing your order. Please try again.'
  } finally {
    isCheckingOut.value = false
  }
}

function handleStartNewOrder() {
  receipt.value = null
  checkoutError.value = ''
}
</script>

<template>
  <template v-if="receipt">
    <Receipt :receipt="receipt" @start-new-order="handleStartNewOrder" />
  </template>

  <template v-else>
    <MenuList @add-to-cart="handleAddToCart" />
    <p v-if="checkoutError" class="checkout-error">{{ checkoutError }}</p>
    <Cart
      :items="cart"
      :is-checking-out="isCheckingOut"
      @increase-item="handleIncreaseItem"
      @decrease-item="handleDecreaseItem"
      @checkout="handleCheckout"
    />
  </template>
</template>

<style scoped>
.checkout-error {
  max-width: 480px;
  margin: 10px auto;
  padding: 10px;
  background: #fdecea;
  color: #b3261e;
  border-radius: 4px;
  text-align: center;
}
</style>