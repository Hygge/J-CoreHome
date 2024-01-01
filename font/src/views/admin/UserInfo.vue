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
            <el-divider>
                基础设置
            </el-divider>
            <el-row  class="settings">
                <el-input class="input-setting"  size="large" v-model="setting.avatar" placeholder="Please input">
                    <template #prepend>头像地址</template>
                </el-input>
                <el-input class="input-setting"  size="large"  v-model="setting.name" placeholder="Please input">
                    <template #prepend>名称</template>
                </el-input>
            </el-row>
            <el-row  class="settings">
                <el-input class="input-setting"  size="large" v-model="setting.sdk" placeholder="Please input">
                    <template #prepend>版本</template>
                </el-input>
                <el-input class="input-setting"  size="large"  v-model="setting.ICP" placeholder="Please input">
                    <template #prepend>备案号</template>
                </el-input>
            </el-row>
            <el-row  class="settings">
                <el-input class="input-setting"  size="large" v-model="setting.year" placeholder="Please input">
                    <template #prepend>时间</template>
                </el-input>
                <el-input class="input-setting"  size="large"  v-model="setting.QQ" placeholder="Please input">
                    <template #prepend>QQ</template>
                </el-input>
            </el-row>
            <el-row  class="settings">
                <el-input class="input-setting"  size="large" v-model="setting.blogs" placeholder="Please input">
                    <template #prepend>博客园</template>
                </el-input>
                <el-input class="input-setting"  size="large"  v-model="setting.github" placeholder="Please input">
                    <template #prepend>GitHub</template>
                </el-input>
            </el-row>
            <el-row  class="settings">
                <el-input class="input-setting"  size="large" v-model="setting.juejin" placeholder="Please input">
                    <template #prepend>掘金</template>
                </el-input>
                <el-input class="input-setting"  size="large"  v-model="setting.bilibli" placeholder="Please input">
                    <template #prepend>哔哩哔哩</template>
                </el-input>
            </el-row>
            <el-row  class="settings">
                <el-input class="input-setting"  size="large" v-model="setting.signature" placeholder="Please input">
                    <template #prepend>签名</template>
                </el-input>
                <el-input class="input-setting"  size="large"  v-model="setting.music" placeholder="Please input">
                    <template #prepend>背景音乐</template>
                </el-input>
            </el-row>
            <el-row  class="settings" style="justify-content: center;">
                <el-button type="success" @click="saveSetting" round>保存设置</el-button>
            </el-row>

        </el-card>
    </el-row>
</template>

<script>
import {onMounted, reactive, toRaw} from "vue";
import {ElMessage, ElMessageBox} from "element-plus";
import {Get, Patch, PatchJson, PostJson} from "@/utils/axios/restful.js";

export default {
    name: "UserInfo",

    setup(){
        // do not use same name with ref
        const form = reactive({
            name: '',
            email: '',
            password: '',
        })
        const setting = reactive({
            avatar: '',
            name: '',
            ICP: '',
            sdk: '',
            signature: '',
            music: '',
            bilibli: '',
            github: '',
            QQ: '',
            blogs: '',
            juejin: '',
            year: ''
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
        const saveSetting = () =>{
            PostJson('/Settings/AddSettings', toRaw(setting)).then(res => {
                if (res.data.code == 200){
                    ElMessage.success("保存成功");
                }
            })

        }
        onMounted( () => {
            Get('/User').then(res => {
                form.name = res.data.data.userInfo.name;
                form.email = res.data.data.userInfo.email;
            })
            Get('/Settings/List').then(res => {
                let data = res.data.data;
                setting.name = data.name;
                setting.ICP = data.ICP;
                setting.avatar = data.avatar;
                setting.blogs = data.blogs;
                setting.music = data.music;
                setting.github = data.github;
                setting.juejin = data.juejin;
                setting.year = data.year;
                setting.QQ = data.QQ;
                setting.bilibli = data.bilibli;
                setting.sdk = data.sdk;
                setting.signature = data.signature;
            })

        })

        return{
            form, onSubmit, setting, saveSetting
        }
    }

}
</script>

<style >
.box{
    border-radius: 20px;
    margin: 0 auto;
    padding: 20px;
    max-width: 100%;
}
.settings{
    padding: 5px;
    justify-content:space-between;
}
.input-setting{
    width: 400px;
    margin: 10px;
}

</style>