<template>
    <div class="footer">
        <p style="padding: 20px;">Designed and Developed by <span style="color: blueviolet">{{ setting.name }} </span> <span style="color: red;">❤</span> Powered by {{setting.sdk }} &nbsp;<el-button text @click="login">登录</el-button></p>
        <p>© {{ year }} - {{ setting.name }} &nbsp;<a href="https://beian.miit.gov.cn/" target="_blank">{{ setting.ICP }}</a></p>
    </div>
</template>

<script>
import {onMounted, reactive, ref} from "vue";
import {useRouter} from "vue-router";
import {Get} from "@/utils/axios/restful.js";

export default {
    name: "Footer",
    setup(){
        const router = useRouter()
        const setting = reactive(JSON.parse(localStorage.getItem('setting')))
        const year = new Date().getFullYear();
        onMounted(()=>{
            Get('/Account/UserInfo').then(res => {
                setting.value = res.data.data.setting;
                // userInfo.value = res.data.data.userInfo;
            })


        })
        const login = () => {
            router.push('/login');
        }

        return{
            setting, year, login
        }
    }

}
</script>

<style scoped>
.footer{
    width:100%;
    height: 7rem;
    bottom:0;
    left:0;
    background-color: #f5f5f5;
    text-align: center;
}

</style>