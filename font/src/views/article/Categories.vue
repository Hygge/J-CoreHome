<template>
  <div>
      <div class="ca-container">
          <div class="ca-content">
              <el-row>
                  <el-card class="ca-card" shadow="never" style="background-color: gainsboro"> 分类 </el-card>
                  <el-card class="ca-card" shadow="hover" v-for="(item, index) in categories" :key="item.cId" @click="getArticlesByCid(item.cId)"> {{ item.categoryName }} </el-card>
              </el-row>
          </div>
          <div class="ca-articles">
              <div class="ca-article" v-for="item in articles" :key="item.arId">
                  <router-link :to="'/index/detail/'+ item.arId"><h2>{{ item.title }}</h2></router-link>
                  <p style="margin-top: 20px;">{{ item.createdTime }} &nbsp;&nbsp; <el-button type="success"> {{ item.categoryName }}</el-button></p>
                  <p style="margin-top: 10px;">{{ item.overview }}</p>
<!--                  <p style="margin-top: 10px;">
                      <el-button type="success" round v-for="(o,index) in item.tags" :key="index">{{ o }}</el-button>
                  </p>-->
                  <el-divider></el-divider>
              </div>
              <el-row class="home-pagination">
                  <el-pagination
                      v-model:current-page="currentPage"
                      v-model:page-size="pageSize"
                      :page-sizes="[15, 25, 30]"
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
      </div>
  </div>

</template>

<script>

import {onMounted, ref} from "vue";
import {useRouter} from "vue-router";
import {Get, PostJson} from "@/utils/axios/restful.js";
import {getCategory} from "@/api/common.js";

export default {
    name: "Categories",
    setup(){

        const router = useRouter();

        const articles = ref([
            {
                arId: 1 ,
                title: 'winform TextBox输入字符串字体间隙问题',
                overview: '摘要： winform TextBox输入字符串字体间隙问题 winform TextBox输入字符串字体间隙问题，解决；输入法问题 ，使用半角 阅读全文',
                createdTime: '2023-07-03 12:30:00',
                number: 99,
                cId : 2,
                categoryName : 'java',
                tags: [ 'winform','Task','多线程']
            },
            {
                arId: 2 ,
                title: 'winform TextBox输入字符串字体间隙问题',
                overview: '摘要： winform TextBox输入字符串字体间隙问题 winform TextBox输入字符串字体间隙问题，解决；输入法问题 ，使用半角 阅读全文',
                createdTime: '2023-07-03 12:30:00',
                number: 99,
                cId : 2,
                categoryName : 'java',
                tags: [ 'winform','Task','多线程']
            },

        ]);
        const categories = ref([ ]);

        const getArticlesByCid = (id) => {
            cId.value =  id;
            getArticleList(currentPage.value, pageSize.value)
        }
        const currentPage = ref(1)
        const pageSize = ref(10)
        const total = ref(0)
        const small = ref(false)
        const background = ref(false)
        const disabled = ref(false)
        const handleSizeChange = (number) => {
            console.log(`${number} items per page`)
        }
        const handleCurrentChange = (number) => {
            console.log(`current page: ${number}`)
        }
        const getCategories = () => {
            getCategory().then( res => {
                categories.value = res.data.data
            })
        }

        const cId = ref(router.currentRoute.value.params.cId);
        const getArticleList = (cur, size) => {
            let data = {
                "current": cur,
                "pageSize": size,
                "id": 0,
                "title": '',
                "categoryId": cId.value
            }
            PostJson('/Articles/ListA',data).then((response) => {
                let data = response.data;
                total.value = data.data.dataCount;
                articles.value = data.data.list;

            })
        }
        onMounted(() => {
            getCategories();

            getArticleList(currentPage.value, pageSize.value)
        })

        return {
            articles, categories, getArticlesByCid, currentPage, pageSize, small, background, disabled, total,
            handleSizeChange, handleCurrentChange
        }

    },


}
</script>

<style scoped>
.ca-container{
    margin : 0 auto;
    width: 80%;
    display: flex;

}
.ca-content{
    width: 25%;
    padding: 20px;


    .ca-card{
        margin-left: 30%;
        width: 100%;
    }
}
.ca-articles{
    margin-left: 30px;
    width: 60%;
    .ca-article{
        padding: 20px;
    }

}

</style>