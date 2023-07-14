<template>
  <div class="login" :style="{ backgroundImage: `url(${this.bgmUrl})` }">

      <el-card>
        <h2 class="h1">Login with Password</h2>
          <el-input type="password" v-model="password" placeholder="Please input password" />
          <el-button type="success" @click="login" round>Login</el-button>
          <hr />
          <footer style="padding: 10px; text-align: center;">
              <p style="margin-bottom: 0;font-weight: bolder;">© {{ year }} - Hygge</p>
          </footer>
      </el-card>
  </div>
</template>

<script>
import {onMounted, ref} from "vue";
import {PostForm} from "@/utils/axios/restful.js";
import {useRouter} from "vue-router";
import {ElMessage} from "element-plus";

export default {
    name: "Login",
    setup(){
        const bgmUrl = ref("https://bing.icodeq.com")
        const password = ref('')
        const router = useRouter();
        const login = () => {
            let data = { 'username': "admin", 'password' : password.value};
            PostForm('/Account/Login', data).then( res => {
                let data = res.data;
                localStorage.setItem("token", data.data);

                router.push("/console/index");
                ElMessage({
                    message: '登录成功 ',
                    type: 'success',
                })
            })

        }
        const year = new Date().getFullYear()
        onMounted( () => {

        })

        return {
            bgmUrl, password, year,
            login
        }
    }
}
</script>

<style scoped>
.login{
    width: 100%;
    height: 100%;
    left: 0;
    bottom: 0;
    position: absolute;
    background-size: cover;
}
.el-card{
    margin: 0 auto;
    margin-top: 12%;
    width: 350px;
    height: 300px;
    border-radius: 20px;
    background-color: rgba(255,255,255,.6);
    box-shadow: 0 0 20px 10px rgba(0,0,0,.15);
    text-align: center;
    line-height: 78px;
    .h1{
        width: 100%;
        justify-content: center;
        padding: 10px;

    }
}
.el-input{
    width: 70%;
    display: block;
    margin: 0 auto;
}
.el-button{
    /*margin-top: 30px;*/
}
</style>