<template>

    <div class="common-layout">
        <el-container>
            <Aside/>
            <el-main>
              <router-view/>
            </el-main>
        </el-container>
    </div>
</template>

<script>
import Aside from "@/components/admin/Aside.vue";

export default {
    name: "Index",
    components: {Aside}
}
</script>

<style scoped>
.common-layout{
    max-width: 100%;
    height: 100%;
}

</style>