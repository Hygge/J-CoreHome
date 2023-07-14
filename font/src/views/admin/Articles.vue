<template>
   <el-row>
       <el-card>
           <h1>文章列表</h1>
           <el-form :inline="true" :model="formInline" class="demo-form-inline">
               <el-form-item label="标题">
                   <el-input v-model="formInline.title" placeholder="文章标题"></el-input>
               </el-form-item>
               <el-form-item label="分类">
                   <el-select v-model="formInline.cId" placeholder="分类">
                       <el-option  v-for="(item, index) in categories" :key="item.cId" :label="item.categoryName" :value="item.cId"></el-option>
                   </el-select>
               </el-form-item>
               <el-form-item>
                   <el-button type="primary" @click="searchArticles">查询</el-button>
                   <el-button type="success" @click="add">新增</el-button>
               </el-form-item>
           </el-form>
           <el-table :data="tableData" style="width: 100%">
               <el-table-column label="编号" width="120">
                   <template #default="scope">
                       <i class="el-icon-time"></i>
                       <span style="margin-left: 10px">{{ scope.row.arId }}</span>
                   </template>
               </el-table-column>
               <el-table-column label="标题" width="240">
                   <template #default="scope">
                       <i class="el-icon-time"></i>
                       <span style="margin-left: 10px;user-select: text;">{{ scope.row.title }}</span>
                   </template>
               </el-table-column>
               <el-table-column label="分类" width="180">
                   <template #default="scope">
                       <i class="el-icon-time"></i>
                       <span style="margin-left: 10px; user-select: text;">{{ scope.row.categoryName }}</span>
                   </template>
               </el-table-column>
               <el-table-column label="浏览量" width="120">
                   <template #default="scope">
                       <i class="el-icon-time"></i>
                       <span style="margin-left: 10px">{{ scope.row.number }}</span>
                   </template>
               </el-table-column>
               <el-table-column label="发布时间" width="180">
                   <template #default="scope">
                       <i class="el-icon-time"></i>
                       <span style="margin-left: 10px">{{ scope.row.createdTime }}</span>
                   </template>
               </el-table-column>
               <el-table-column label="操作">
                   <template #default="scope">
                       <router-link :to="'/console/addArticle/'+ scope.row.arId">
                           <el-button size="small">
                              编辑
                           </el-button>
                       </router-link>
                       &nbsp;
                       <el-button size="small" :type="scope.row.isPublic == true ? 'warning' : 'success' " @click="handleIsPublic(scope.$index, scope.row)">
                           {{ scope.row.isPublic == true ? '回收' : '发布' }}
                       </el-button>
                       <el-popconfirm title="Are you sure to delete this?" @confirm="handleDelete(scope.$index, scope.row)">
                           <template #reference>
                               <el-button size="small"  type="danger">删除</el-button>
                           </template>
                       </el-popconfirm>
                   </template>
               </el-table-column>
           </el-table>
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
       </el-card>
   </el-row>
</template>

<script>
import {onMounted, ref} from "vue";
import {Delete, Get, Patch, PatchJson, PostJson} from "@/utils/axios/restful.js";
import { useRouter } from 'vue-router';
import {ElMessage} from "element-plus";
import {getArticles, getCategory} from "@/api/common.js";

export default {
    name: "Articles",
    setup(){
        const router = useRouter();
        const categories = ref([  ]);
        const  formInline = ref({
            title: '',
            cId: 0,
        })
        const tableData = ref([ ]);
        const handleEdit = (index, row) => {
            console.log(index, row)
            const url = router.resolve({
                path:  'addArticle',
            });
            // 打开新窗口
            window.open(url.href);

        };
        const handleIsPublic = (index, row) => {
            PatchJson('/Articles/' + row.arId,
                {

                    "title": '',
                    "Content": '',

                    "isPublic": !row.isPublic,
                }
                // {'IsPublic': !row.isPublic }
        ).then( res => {
                if (res.data.code == 200){
                    if (row.isPublic)
                        ElMessage.success("回收成功！");
                    else
                        ElMessage.success("发布成功！");
                    getArticle(currentPage.value, pageSize.value);
                }
            })

        };

        const handleDelete = (index, row) => {
            Delete('/Articles/'+ row.arId , null).then( res => {
                if (res.data.code == 200)
                ElMessage.success("删除成功！");
                getArticle(currentPage.value, pageSize.value);
            })

        };

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
                 "id": 0,
                 "title": formInline.value.title,
                 "categoryId": formInline.value.cId
            }
            getArticles(data).then((response) => {
                let data = response.data;
                total.value = data.data.dataCount;
                tableData.value = data.data.list;

            }).catch((error) => {
                console.log(error);
            });
        }
        const searchArticles = () => {

            getArticle(currentPage.value, pageSize.value);
        }
        const add = () =>{
            const url = router.resolve({
                path:  'addArticle/0',
            });
            // 打开新窗口
            window.open(url.href);

        }

        onMounted(()=> {
            getArticle(currentPage.value, pageSize.value);

            // 加载分类
            getCategory().then( res => {
                categories.value = res.data.data
            })

        })
        return {
            tableData, currentPage, pageSize, small, background, disabled, total, formInline, categories,
            handleSizeChange, handleCurrentChange,handleEdit, handleDelete, searchArticles, add,handleIsPublic,
        }

    }

}
</script>

<style scoped>
.el-card{
    border-radius: 50px;
    width: 70%;
    margin: 0 auto;
    text-align: center;
}
.el-form{
    margin-top: 20px;
}
.home-pagination{
    margin-left: 20px;
    margin-top: 20px;

}
</style>