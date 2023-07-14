<template>
    <el-row>

        <el-card class="box">
            <h1>列表</h1>
            <el-table :data="tableData" style="width: 100%">
                <el-table-column prop="cId" label="id" width="100"> </el-table-column>
                <el-table-column prop="categoryName" label="类名" width="180"> </el-table-column>
                <el-table-column label="操作">
                    <template #default="scope">
                        <el-popconfirm title="Are you sure to delete this?" @confirm="handleDelete(scope.$index, scope.row)">
                            <template #reference>
                                <el-button size="small"  type="danger">删除</el-button>
                            </template>
                        </el-popconfirm>
                    </template>
                </el-table-column>
            </el-table>

        </el-card>

        <el-card class="box">
            <el-form :inline="true" :model="formInline" class="demo-form-inline">
                <el-form-item label="类名：">
                    <el-input v-model="formInline.title" placeholder="请输入分类名称"></el-input>
                </el-form-item>
                <el-form-item>
                    <el-button type="success" @click="add">保存</el-button>
                </el-form-item>
            </el-form>

        </el-card>
    </el-row>
</template>

<script>
import {onMounted, ref} from "vue";
import {Delete, Get, PostForm} from "@/utils/axios/restful.js";
import {ElMessage} from "element-plus";
import {getCategory} from "@/api/common.js";

export default {
    name: "Categories",
    setup(){
        const tableData = ref([]);
        const handleDelete = (index, row) => {
            Delete('/Category/'+ row.cId , null).then( res => {
                if (res.data.code == 200)
                    ElMessage.success("删除成功！");
                getList();
            })

        };
        const  formInline = ref({
            title: '',
        })
        const add = () => {

            PostForm('/Category', {categoryName: formInline.value.title}).then(res => {
                formInline.value.title = '';
                getList();
            })
        }

        const getList = () => {
            getCategory().then( res => {
                console.log(res.data.data)
                tableData.value = res.data.data
            })
        }

        onMounted(() => {
            // 加载分类
            getList();
        })

        return {
            tableData, handleDelete, formInline, add
        }
    }
}
</script>

<style scoped>
.box{
    border-radius: 20px;
    margin-left: 20px;
}

</style>