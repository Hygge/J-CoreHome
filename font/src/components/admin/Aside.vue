<template>
    <el-aside>
        <div class="as-head" style="text-align: center;">
            <el-link href="/index/home"  target="_blank">HYGGE | <el-icon><HomeFilled /></el-icon></el-link>
        </div>
        <div class="as-menu">
            <ul class="as-ul">
                <li><router-link to="/console/article"><el-icon><Notebook /></el-icon>&nbsp;文章管理</router-link> </li>
                <li><router-link to="/console/categories"><el-icon><Notebook /></el-icon>&nbsp;分类管理</router-link></li>
                <li><router-link to="/console/friends"><el-icon><Notebook /></el-icon>&nbsp;友链管理</router-link></li>
                <li><router-link to="/console/index"><el-icon><Notebook /></el-icon>&nbsp;基础设置</router-link></li>
                <li><router-link to="/console/userInfo"><el-icon><Notebook /></el-icon>&nbsp;个人设置</router-link></li>
                <li><router-link to="/console/fileList"><el-icon><Notebook /></el-icon>&nbsp;附件管理</router-link></li>
            </ul>
        </div>

        <div style="text-align: center;margin-top: 20px;">
            <el-avatar :size="100" :src="avatar" />
            <br/>
            <el-button type="danger" @click="logout" > 退出登录 </el-button>
        </div>
        <br/>
        <br/>
    </el-aside>
</template>

<script>
import {HomeFilled, Notebook} from "@element-plus/icons";
import {onMounted, ref} from "vue";
import {Get} from "@/utils/axios/restful.js";
import {useRouter} from "vue-router";
import {ElNotification} from "element-plus";

export default {
    name: "Aside",
    components: {Notebook, HomeFilled},
    setup(){
        const router = useRouter();
        const avatar = ref("");

        const logout = () => {
          Get('/Account/LogOut').then(res => {
              ElNotification({
                  title: 'Success',
                  message: '注销成功！',
                  type: 'success',
              })
              router.push('/');
              localStorage.removeItem('token');
          })
        }

        onMounted(()=> {
            Get('/User').then(res => {

                avatar.value = res.data.data.setting.avatar;

            })
        })

        return{
            avatar, logout,
        }
    }
}
</script>

<style scoped>

.el-aside{
    width: 10%;
    height: 100%;
    background-color: rgba(255,255,255,.6);
    box-shadow: 0 0 20px 10px rgba(253, 5, 5, 0.15);

    .as-head .el-link{
        height: 100px;
        color: black;
        text-decoration: none;
        font-size: 25px;
        justify-content: center;
    }

    .as-menu{
        height: 70%;

    }

}
.as-ul{
    line-height: 50px;
    text-align: center;

}
.as-ul li:hover{
    color: red;
}

</style>