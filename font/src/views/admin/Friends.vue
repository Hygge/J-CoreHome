<template>

  <el-row>
      <el-card>
          <h1>好友列表</h1>
          <el-form :inline="true" :model="formInline" class="demo-form-inline">
              <el-form-item label="名称">
                  <el-input v-model="formInline.title" placeholder="好友用户名"></el-input>
              </el-form-item>
              <el-form-item>
                  <el-button type="primary" @click="searchFriend">查询</el-button>
                  <el-button type="success" @click="centerDialogVisible = true">新增</el-button>
              </el-form-item>
          </el-form>
          <el-table :data="tableData" style="width: 100%;" >
              <el-table-column label="头像" width="180">
                  <template #default="scope">
                      <el-avatar :size="50" :src="scope.row.fAvatar" />
                  </template>
              </el-table-column>
              <el-table-column  label="名称" width="180">
                  <template #default="scope">
                      <i class="el-icon-time"></i>
                      <span style="margin-left: 10px;user-select: text;">{{ scope.row.fUserName }}</span>
                  </template>
              </el-table-column>
              <el-table-column label="签名" width="280" >
                  <template #default="scope">
                      <i class="el-icon-time"></i>
                      <span style="margin-left: 10px;user-select: text;">{{ scope.row.fSign }}</span>
                  </template>
              </el-table-column>
              <el-table-column label="链接" width="120">
                  <template #default="scope">
                      <el-link type="success" :href="scope.row.fUrl" target='_blank'>过去看看</el-link>

                  </template>
              </el-table-column>
              <el-table-column label="操作">
                  <template #default="scope">
                        <el-button  size="small" @click="handleEdit(scope.$index, scope.row)">
                            编辑
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

      <el-dialog
          v-model="centerDialogVisible"
          title="添加友链信息"
          width="30%"
          align-center
      >  <el-form class="f-add" :model="form" label-width="120px">
          <el-form-item label="名称">
              <el-input v-model="form.name" />
          </el-form-item>
          <el-form-item label="链接">
              <el-input type="text" v-model="form.url" placeholder="请输入好友博客链接" />
          </el-form-item>
          <el-form-item label="签名">
              <el-input type="text" v-model="form.sign" />
          </el-form-item>
          <el-form-item label="头像">
              <el-input type="text" v-model="form.avatar" placeholder="请输入头像链接" />
          </el-form-item>

      </el-form>
          <template #footer>
      <span class="dialog-footer">
        <el-button @click="centerDialogVisible = false">Cancel</el-button>
        <el-button type="primary" @click="add">
          Confirm
        </el-button>
      </span>
          </template>
      </el-dialog>
  </el-row>

</template>

<script>
import {useRouter} from "vue-router";
import {onMounted, reactive, ref} from "vue";
import {Delete, Get, PatchJson, PostJson} from "@/utils/axios/restful.js";
import {ElMessage} from "element-plus";

export default {
    name: "Friends",

    setup(){
        const router = useRouter();
        const categories = ref([  ]);
        const  formInline = ref({
            fUserName: '',
        })
        const tableData = ref([
            { fUserName : 'Hygge', fUrl: 'sxxx', fSign: 'sxxx', fId : 0, fAvatar: 'xx'}
        ]);
        const handleEdit = (index, row) => {
            centerDialogVisible.value = true
            form.name = row.fUserName;
            form.avatar = row.fAvatar;
            form.sign = row.fSign;
            form.url = row.fUrl;
            form.id = row.fId;
        };

        const handleDelete = (index, row) => {
            console.log(index, row)
            Delete('/Friend/'+ row.fId , null).then( res => {
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
                "size": size,
            }
            if (formInline.value.fUserName !== ''){
                data.name = formInline.value.fUserName;
            }
            Get("/Friend", data).then((response) => {
                let data = response.data;
                total.value = data.data.dataCount;
                tableData.value = data.data.list;
            }).catch((error) => {
                console.log(error);
            });
        }
        const searchFriend = () => {
            console.log(formInline.value.fUserName)
            getList(currentPage.value, pageSize.value);
        }
        const form = reactive({
            avatar: '',
            name: '',
            url: '',
            sign: '',
            id: 0,
        })
        const add = () =>{
            let data = {
                'FId': form.id,
                'FUrl': form.url,
                'FAvatar': form.avatar,
                'FUserName': form.name,
                'FSign': form.sign,
                'IsDelete': false,
            }
            if (form.id == 0){
                PostJson('/Friend', data).then(res => {

                    if (res.data.code ==  200){
                        ElMessage.success("添加成功");
                        centerDialogVisible.value = false
                        resetForm();
                        getList(currentPage.value, pageSize.value);
                    }
                })
            }else {
                PatchJson('/Friend/' + form.id, data ).then(res => {
                    if (res.data.code == 200){
                        ElMessage.success("修改成功");
                        centerDialogVisible.value = false
                        resetForm();
                        getList(currentPage.value, pageSize.value);
                    }
                })
            }

        }
        const resetForm = () => {
            form.sign = '';
            form.url = '';
            form.name = '';
            form.avatar = '';
            form.id = 0;
        }


        onMounted(()=> {
            getList(currentPage.value, pageSize.value);

        })
        return {
            tableData, currentPage, pageSize, small, background, disabled, total, formInline, categories, centerDialogVisible,
            form,
            handleSizeChange, handleCurrentChange,handleEdit, handleDelete, searchFriend, add,
        }

    }

}
</script>

<style scoped>
.el-card{
    border-radius: 50px;
    width: 60%;
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
.f-add{
    width: 80%;
}
</style>