<script setup>
import {onMounted} from "vue";
import {getSetting} from "@/api/common.js";

onMounted(() => {
    // 加载配置
    getSetting().then( res => {
        let data = res.data.data
        localStorage.setItem('setting', JSON.stringify(data))
    })

})

</script>

<template>
    <router-view/>
</template>

<style scoped>
.logo {
  height: 6em;
  padding: 1.5em;
  will-change: filter;
}
.logo:hover {
  filter: drop-shadow(0 0 2em #646cffaa);
}
.logo.vue:hover {
  filter: drop-shadow(0 0 2em #42b883aa);
}
</style>
