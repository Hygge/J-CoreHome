<template>
  <div class="content">
      <div class="wallpaper" :style="{ backgroundImage: `url(${this.bgmUrl})` }">

      </div>
      <div class="home-content">
          <div class="home-article">
              <el-row class="article-row" v-for="(item, index) in articles" :key="item.arId">
                  <el-card class="box-card">
                      <template #header>
                          <div class="card-header">
                              <router-link :to="'/index/detail/'+ item.arId"><h4>{{ item.title }}</h4></router-link>
                              <el-button class="button" text> {{ item.createdTime }}</el-button>
                          </div>
                      </template>
                      <div>摘要： {{ item.overview }}</div>
                  </el-card>
              </el-row>
              <el-row class="home-pagination">
                  <el-pagination
                      v-model:current-page="currentPage"
                      v-model:page-size="pageSize"
                      :page-sizes="[10, 15, 30]"
                      :small="small"
                      :disabled="disabled"
                      :background="background"
                      layout="total, sizes, prev, pager, next, jumper"
                      :total="total"
                      @size-change="handleSizeChange"
                      @current-change="handleCurrentChange"
                  />
              </el-row>


          </div>
          <div class="home-aside">

             <Aside/>

          </div>

      </div>

      <Footer/>
  </div>

</template>

<script>
import {onMounted, ref} from "vue";
import Footer from "@/components/layout/Footer.vue";
import Aside from "@/components/home/Aside.vue";
import {Get, PostJson} from "@/utils/axios/restful.js";


export default {
    name: "index",
    components: {Aside, Footer},

    setup() {
        let bgmUrl = ref('https://bing.icodeq.com');
        let avatar = ref("https://s.cn.bing.net/th?id=OHR.ZhangyeGeopark_ZH-CN1045536243_1920x1080.webp&qlt=50");

        let articles = ref([ ])
        const userInfo = ref({})
        const currentPage = ref(1)
        const pageSize = ref(6)
        const total = ref(0)
        const small = ref(false)
        const background = ref(false)
        const disabled = ref(false)

        const handleSizeChange = (number) => {
            getArticle(currentPage.value, number);
        }
        const handleCurrentChange = (number) => {
            getArticle(number, pageSize.value);
        }

        // 获取文章列表
        const getArticle = (cur, size) => {
            let data = {
                "current": cur,
                "pageSize": size,
                // "id": 0,
                // "title": "string",
                // "categoryId": 0
            }
            PostJson('/Articles/ListA',data).then((response) => {
                let data = response.data;
                total.value = data.data.dataCount;
                articles.value = data.data.list;

            }).catch((error) => {
                console.log(error);
            });
        }

        onMounted(()=> {
            getArticle(currentPage.value, pageSize.value);
            Get('/Account/UserInfo').then(res => {
                avatar.value = res.data.data.setting.avatar;
                // bgmUrl.value = res.data.data.setting.bgmUrl;
                userInfo.value = res.data.data.userInfo;
                console.log(userInfo)


            })

        })
        return {
            bgmUrl, avatar, articles, currentPage, pageSize, small, background, disabled, total,
            handleSizeChange, handleCurrentChange
        }
    },

}
</script>

<style >
.content{
    width: 100%;
    left: 0;
    right: 0;

    .wallpaper{
        width: 100%;
        height: 62rem;

        background-size: cover;
        background-position: 50%;
        background-attachment: fixed;


    }

    .home-content{
        width: 80%;
        height: 100%;
        margin: 0 auto;
        padding: 2%;
        display: flex;

    }

}

.home-content{

    .home-article{
        width: 65%;

        .home-pagination{
            margin-left: 20px;
            margin-top: 20px;

        }
    }

    .home-aside{
        margin-left: 5%;
        width: 30%;
    }

}
.article-row{
    width: 96%;
    padding: 2%;
    .card-header {
        display: flex;
        justify-content: space-between;
        align-items: center;
    }
    .box-card {
        width: 100%;

    }
}

.home-aside{
    padding: 10px;


}

</style>