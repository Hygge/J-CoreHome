
<template>
    <div class="common-layout">

        <div class="header">
            <ul class="header-ul">
                <li><router-link to="/index/home" >首页</router-link></li>
                <li><router-link to="/index/Categories/0" >文章</router-link></li>
                <li>
                    <el-dropdown trigger="click">
                      <span style="font-size: 16px;color: #333; line-height: 4rem; ">
                        分类
                      </span>
                        <template #dropdown>
                            <el-dropdown-menu >
                                <el-dropdown-item v-for="(item, index) in categories" :key="item.cId">
                                    <router-link :to="'/index/Categories/'+ item.cId">{{item.categoryName}}</router-link>
                                </el-dropdown-item>
                            </el-dropdown-menu>
                        </template>
                    </el-dropdown>
                </li>
                <li><router-link to="/index/home" >关于我</router-link></li>
                <li style="margin-left: 10%;"><router-link to="/index/home" >小工具</router-link></li>
            </ul>

        </div>
        <div class="main">
            <router-view></router-view>
        </div>

    </div>
</template>

<script>
import Footer from "@/components/layout/Footer.vue";
import {onMounted, ref} from "vue";
import {Get} from "@/utils/axios/restful.js";
import {getCategory} from "@/api/common.js";
export default {
    name: "index",
    components: {Footer, },

    setup(){
        const categories = ref([  ]);
        onMounted(() => {
            // 加载分类
            getCategory().then( res => {
                categories.value = res.data.data
            })

        })
        return{
            categories,
        }
    }


}
</script>

<style >
.header {
    top: 0;
    position: relative;
    width: 100%;
    height: 4rem;
    line-height: 4rem;
    background-color: white;
    /*box-shadow: 0 0 5px 1px #999;*/
    box-shadow: rgba(0, 0, 0, 0.5) 0px 2px 10px 0px;
    .header-ul{
        margin-left: 10%;
    }
    .header-ul li{
        display: inline-block;
        margin-left: 3%;
        font-size: 16px;
        font-weight: bold;

    }
}
.main{
    height: 100%;
    bottom: 0;
    left: 0;
    background-color: white;

}

.el-dropdown-menu__item  {
    font-size: 16px;
    color: #333;
}

</style>