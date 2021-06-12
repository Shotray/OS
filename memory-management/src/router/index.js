import Vue from 'vue'
import VueRouter from 'vue-router'

Vue.use(VueRouter)

const routes = [
  {
    path: '/FIFO',
    name: 'FIFO',
    component: () => import(
      '../views/FIFO.vue')
  },
  {
    path: '/LRU',
    name: 'LRU',
    // route level code-splitting
    // this generates a separate chunk (about.[hash].js) for this route
    // which is lazy-loaded when the route is visited.
    component: () => import(/* webpackChunkName: "about" */ '../views/LRU.vue')
  }
]

const router = new VueRouter({
  routes
})

export default router
