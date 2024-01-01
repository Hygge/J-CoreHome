<template>
<el-row>
        <el-card>
            <el-input type="text" v-model="fileName" placeholder="请输入关键字" style="width: 10%;"/>
            <el-button type="success" style="margin-left: 20px;" @click="search">搜索</el-button>
            <el-button type="default" style="margin-left: 20px;" @click="add">上传</el-button>

            <el-divider content-position="right">文件列表</el-divider>
            <el-table :data="tableData" style="width: 100%;" >
                <el-table-column label="预览" width="180">
                    <template #default="scope">
                        <el-image style="width: 100px; height: 100px"  :src=" b +scope.row.url" fit="scale-down" />
                    </template>
                </el-table-column>
                <el-table-column  label="文件名称" width="280">
                    <template #default="scope">
                        <i class="el-icon-time"></i>
                        <span style="margin-left: 10px;user-select: text;">{{ scope.row.fileName }}</span>
                    </template>
                </el-table-column>
                <el-table-column label="存储策略" width="180" >
                    <template #default="scope">
                        <span style="margin-left: 10px;user-select: text;"></span>
                        <el-tag type="success" >{{ scope.row.local == true ? '本地' : '云'}}</el-tag>
                    </template>
                </el-table-column>
                <el-table-column label="大小" width="180" >
                    <template #default="scope">
                        <span style="margin-left: 10px;user-select: text;">{{ (scope.row.size/1024/1024).toFixed(2) }}mb</span>
                    </template>
                </el-table-column>

                <el-table-column label="链接" width="180">
                    <template #default="scope">
                        <el-link type="success" :href=" b +scope.row.url" target='_blank'>过去看看</el-link>
                    </template>
                </el-table-column>
                <el-table-column label="上传时间" width="250" >
                    <template #default="scope">
                        <i class="el-icon-time"></i>
                        <span style="margin-left: 10px;user-select: text;">{{ scope.row.createdTime }}</span>
                    </template>
                </el-table-column>

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

    <el-dialog
        v-model="centerDialogVisible"
        title="上传文件"
        width="30%"
        align-center
    >

        <el-upload
            class="upload-demo"
            :action="b +'/api/Common/UpFile'"
            :headers="header"
            :on-preview="handlePreview"
            :on-remove="handleRemove"
            :file-list="fileList"
            list-type="picture"
        >
            <el-button size="small" type="primary">点击上传</el-button>
            <template #tip>
                <div class="el-upload__tip">只能上传 jpg/png 文件，且不超过 5mb</div>
            </template>
        </el-upload>


    </el-dialog>
</el-row>
</template>

<script>
import {onMounted, reactive, ref} from "vue";
import {Delete, Get, PatchJson, PostJson} from "@/utils/axios/restful.js";
import {ElMessage} from "element-plus";
import search from "@element-plus/icons/lib/Search.js";
import {BASE_URL} from "@/utils/axios/index.js";
export default {
    name: "FileList",
    computed: {
        search() {
            return search
        }
    },

    setup(){
        const b = ref(BASE_URL);
        const header =ref( {
            token: localStorage.getItem('token')
        })
        const tableData = ref([
            { fileName : 'Hygge', url: 'sxxx', local: true, id : 0, type: 1,
                createdTime: '2023-12-30 12:30:11', size : 2901923 ,suffix: ''}
        ]);
        const fileList = ref(
            [

            ]
        )
        const handleDelete = (index, row) => {
            console.log(row.id)
            Get('/Common/Delete' , {id: row.id}).then( res => {
                if (res.data.code == 200)
                    ElMessage.success("删除成功！");
                getList(currentPage.value, pageSize.value);
            })

        };

        const currentPage = ref(1)
        const pageSize = ref(6)
        const total = ref(0)
        const small = ref(false)
        const background = ref(false)
        const disabled = ref(false)

        const handleSizeChange = (number) => {
            getList(currentPage.value, number);
        }
        const handleCurrentChange = (number) => {
            getList(number, pageSize.value);
        }

        const centerDialogVisible = ref(false)
        const getList = (cur, size) => {
            let data = {
                "current": cur,
                "pageSize": size,
            }
            if (fileName.value !== ''){
                data.name = fileName.value;
            }
            Get("/Common/GetList", data).then((response) => {
                let data = response.data;
                total.value = data.data.dataCount;
                tableData.value = data.data.list;
            }).catch((error) => {
                console.log(error);
            });
        }
        const fileName = ref('')
        const search = () => {
            console.log(fileName.value)
            getList(currentPage.value, pageSize.value);
        }

        const add = () =>{
            centerDialogVisible.value = true;
            getList(currentPage.value, pageSize.value);
        }
        const resetForm = () => {
            fileName.value = '';
        }
        const handleRemove= (file, fileList)  => {
            console.log(file.name, fileList)
        }

        const handlePreview = (file) => {
            console.log(file)
        }

        onMounted(()=> {
            getList(currentPage.value, pageSize.value);

        })
        return {
            tableData, currentPage, pageSize, small, background, disabled, total,  centerDialogVisible,
            fileName, fileList,header,b,
            handleSizeChange, handleCurrentChange, handleDelete, search, add, handleRemove,  handlePreview
        }
    }
}
</script>

<style scoped>
.el-card{
    margin: 0 auto;
    border-radius: 20px;
}
</style>