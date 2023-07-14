<template>
    <el-header>
        <ul class="header-ul">
            <li><router-link to="/home" >Home</router-link></li>
            <li><router-link to="/home" >Article</router-link></li>
            <li><router-link to="/home" >Category</router-link></li>
            <li><router-link to="/home" >About</router-link></li>
        </ul>
    </el-header>
</template>

<script>
export default {
    name: "Header"
}
</script>

<style >
.el-header {
    width: 100%;
    height: 10%;
    position: fixed;
    top: 0;
    left: 0;
    background-color: aliceblue;
    justify-content: center;
    text-align: center;

    .header-ul li{
        display: inline-block;
        margin-left: 2%;
    }
}


</style>