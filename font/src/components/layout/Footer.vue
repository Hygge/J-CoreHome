<template>
    <div class="footer">
        <p style="padding: 20px;">Designed and Developed by Hygge <span style="color: red;">❤</span> Powered by vue3 + .NET 6 &nbsp;<el-button text @click="login">登录</el-button></p>
        <p>© {{ year }} - Hygge &nbsp;{{ setting.icPnumber }}</p>
    </div>
</template>

<script>
import {onMounted, ref} from "vue";
import {useRouter} from "vue-router";
import {Get} from "@/utils/axios/restful.js";

export default {
    name: "Footer",
    setup(){
        const router = useRouter()
        const setting = ref({
            bgmUrl: '',
            avatar: '',
            icPnumber: '赣ICP备2023004722号',
        })
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