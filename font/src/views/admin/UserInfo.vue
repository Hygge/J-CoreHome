<template>
    <el-row>

        <el-card class="box">

            <el-form :model="form" label-width="120px">
                <el-form-item label="用户名">
                    <el-input v-model="form.name" />
                </el-form-item>
               <el-form-item label="邮箱">
                    <el-input type="email" v-model="form.email" />
                </el-form-item>
               <el-form-item label="密码">
                    <el-input type="password" v-model="form.password" />
                </el-form-item>

                <el-form-item>
                    <el-button type="primary" @click="onSubmit">修改</el-button>
                    <el-button>取消</el-button>
                </el-form-item>
            </el-form>
        </el-card>
    </el-row>
</template>

<script>
import {onMounted, reactive} from "vue";
import {ElMessage, ElMessageBox} from "element-plus";
import {Get, Patch, PatchJson} from "@/utils/axios/restful.js";

export default {
    name: "UserInfo",

    setup(){
        // do not use same name with ref
        const form = reactive({
            name: '',
            email: '',
            password: '',
        })

        const onSubmit = () => {
            ElMessageBox.confirm(
                '确定修改，继续提交？',
                'Warning',
                {
                    confirmButtonText: '确定',
                    cancelButtonText: '取消',
                    type: 'warning',
                }
            ).then(() => {
                let data = {
                    'username' : form.name,
                    'email' : form.email,
                    'password' : form.password,
                }
                Patch('/User/', data ).then(res => {
                    if (res.data.code == 200){
                        ElMessage.success("修改成功");
                    }
                })

                }).catch(() => {
                    ElMessage({
                        type: 'info',
                        message: 'Delete canceled',
                    })
                })
        }
        onMounted( () => {
            Get('/User').then(res => {
                form.name = res.data.data.userInfo.name;
                form.email = res.data.data.userInfo.email;
            })

        })

        return{
            form, onSubmit,
        }
    }

}
</script>

<style scoped>
.box{
    border-radius: 20px;
}
</style>