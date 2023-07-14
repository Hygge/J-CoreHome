<template>
  <div style="background-color: #888888;padding: 50px; background-size: cover;" :style="{ backgroundImage: `url(${this.bgmUrl})` }">
      <el-row style="height: 100px;">

      </el-row>
      <el-card  >
          <div>
              <el-avatar :size="50" :src="avatar" />
              <span style="font-weight: bolder;">{{ userInfo.name}}</span>

              发布于 {{ article.createdTime }}
              /
              {{ article.number }} 阅读
              /
              {{ article.number }} 评论
              </div>
          <h2 style="margin-top: 20px">{{ article.title }}</h2>
          <el-divider content-position="right">Rabindranath Tagore</el-divider>
          <div id="content" v-html="article.content">

          </div>


      </el-card>
  </div>

</template>

<script>
import {onMounted, ref, shallowRef} from "vue";
import {Get} from "@/utils/axios/restful.js";
import {useRouter} from "vue-router";
import user from "@element-plus/icons/lib/User.js";
import {Editor, Toolbar} from "@wangeditor/editor-for-vue";

export default {
    name: "detail",
    components: {Toolbar, Editor},
    computed: {
        user() {
            return user
        }
    },

    setup(){
        let bgmUrl = ref("https://bing.icodeq.com");

        const router = useRouter();
        const html = ref('<p>aaaaaaaaaaaaaaaaa</p>')

        const article = ref({});
        const id = ref(router.currentRoute.value.params.arId);
        const userInfo = ref({})
        const avatar = ref("");


        const getArticle = () => {
          Get('/Articles/'+ id.value).then(res => {
              console.log( res.data.data)
              article.value = res.data.data;

          })
        }

        onMounted(() => {
            getArticle();
            Get('/Account/UserInfo').then(res => {
                avatar.value = res.data.data.setting.avatar;
                userInfo.value = res.data.data.userInfo;
                console.log(userInfo)


            })
        })

        return{
            html, article, avatar, userInfo, bgmUrl
        }
    }

}
</script>

<style scoped>
.el-card{
    width: 60%;
    margin: 0 auto;
    border-radius: 10px;
    z-index: 999;
    user-select: text;
}
#content{
  padding: 20px;
}

</style>