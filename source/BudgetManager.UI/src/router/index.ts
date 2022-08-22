import BudgetPage from "@/pages/BudgetPage.vue";
import LoginPage from "@/pages/LoginPage.vue";
import { createRouter, createWebHistory } from "vue-router";

const routes = [
  {
    path: "/",
    redirect: () => ({ path: "/login" }),
  },
  {
    path: "/home",
    name: "BudgetPage",
    component: BudgetPage,
  },
  {
    path: "/login",
    name: "LoginPage",
    component: LoginPage,
  },
];

const router = createRouter({
  history: createWebHistory(),
  routes,
});

export default router;
