<script setup>
import { ref } from 'vue'
import MenuList from './components/MenuList.vue'
import Cart from './components/Cart.vue'

const cart = ref([])

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
</script>

<template>
  <MenuList @add-to-cart="handleAddToCart" />
  <Cart
    :items="cart"
    @increase-item="handleIncreaseItem"
    @decrease-item="handleDecreaseItem"
  />
</template>