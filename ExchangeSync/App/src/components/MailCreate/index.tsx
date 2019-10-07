import * as React from 'react';
import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { Stack, IStackProps } from 'office-ui-fabric-react/lib/Stack';
import { PrimaryButton, Label } from 'office-ui-fabric-react';
import axios from "axios";

import { TagPicker, IBasePicker, ITag } from 'office-ui-fabric-react/lib/Pickers';


export interface IMailCreateProps {
    reply: boolean,
    mailId: string
}

export default class MailCreate extends React.Component<IMailCreateProps, any> {

    constructor(props: IMailCreateProps) {
        super(props);
        var cporps = props as any;
        if (cporps.staticContext && cporps.staticContext.data) { //服务器渲染(用于回复其他人的邮件)
            let data = cporps.staticContext.data;
            this.state = {
                reply: true,
                replyloading: false,
                title: data.title,
                content: data.content,
                reciver: data.reciver,
                copyto: data.copyto,
            }
        } else if ((window as any).reply) {         //客户端二次渲染(用于回复其他人的邮件)
            let data = (window as any).reply;
            this.state = {
                reply: true,
                replyloading: false,
                title: data.title,
                content: data.content,
                reciver: data.reciver,
                copyto: data.copyto,
            }
        } else {                                    //客户端无状态渲染
            this.state = {
                reply: (this.props as any).match.params.mailId ? true : false,
                replyloading: (this.props as any).match.params.mailId ? true : false,
                title: "",
                content: "",
            };
        }
    }

    private _onFilterChanged = (filterText: string, tagList: ITag[]): ITag[] => {
        var promise = this._searchUser(filterText) as any;
        return promise;
    };

    private _getTextFromItem(item: ITag): string {
        return item.name;
    }

    private _searchUser(keyword: string): any {
        return new Promise(function (resolve, reject) {
            axios.get("/user/" + keyword).then(function (response) {
                var data = response.data;
                resolve(data);
            })
        })
    }

    private _handleChange(name: string, items: ITag[]) {
        this.setState({ [name]: items });
    }

    private _handleInputChange(name: string, e: any) {
        this.setState({
            [name]: e.target.value
        })
    }

    private _handleSend(e: any) {
        e.preventDefault();
        let state = this.state as any;
        if (!state["reciver"] || state["reciver"].length == 0) {
            alert("请选择收件人!");
            return;
        }
        if (!state["title"]) {
            alert("请输入邮件主题");
            return;
        }
        if (!state["content"]) {
            alert("请输入邮件内容");
            return;
        }
        let data = {
            title: state["title"],
            content: state["content"],
            reciver: state["reciver"][0].key,
            copyTo: state["copyto"].map((item: any) => item.key)
        };
        console.log(data);
    }

    componentDidMount() {
        if (this.state.reply) {
            var data = {
                title: "这是客户端的标题",
                reciver: [{ key: "v-ms-kz@scrbg.com", name: "康泽(252525)" }],
                copyto: "",
                content: "这是客户端的内容"
            }
            this.setState({
                reply: true,
                replyloading: false,
                title: "[回复]" + data.title,
                content: "\r\r\r\r" + data.content,
                reciver: data.reciver,
                copyto: data.copyto,
            })
        }
    }


    public render() {
        if (this.state.reply && this.state.replyloading) {
            return (
                <div>
                    正在加载...
                </div>
            );
        }
        return (
            <Stack tokens={{ childrenGap: 20 }} styles={{ root: { width: "100%" } }}>
                <Label>&nbsp;&nbsp;&nbsp;收件人：</Label>
                <TagPicker
                    styles={{ root: { marginTop: "0px !important" }, text: { borderTopStyle: "none", borderRightStyle: "none", borderLeftStyle: "none" } }}
                    onResolveSuggestions={this._onFilterChanged}
                    getTextFromItem={this._getTextFromItem}
                    pickerSuggestionsProps={{
                        suggestionsHeaderText: '请输入姓名/邮件地址',
                        noResultsFoundText: '没有找到该用户'
                    }}
                    selectedItems={this.state.reciver}
                    itemLimit={1}
                    onChange={this._handleChange.bind(this, "reciver")}
                />

                <Label>&nbsp;&nbsp;&nbsp;抄送(可选):</Label>
                <TagPicker
                    styles={{ root: { marginTop: "0px !important" }, text: { borderTopStyle: "none", borderRightStyle: "none", borderLeftStyle: "none" } }}
                    onResolveSuggestions={this._onFilterChanged}
                    getTextFromItem={this._getTextFromItem}
                    pickerSuggestionsProps={{
                        suggestionsHeaderText: '请输入姓名/邮件地址',
                        noResultsFoundText: '没有找到该用户'
                    }}
                    itemLimit={100}
                    onChange={this._handleChange.bind(this, "copyto")}
                />      
                <TextField label="主题:" underlined value={this.state.title} onChange={this._handleInputChange.bind(this, "title")} />
                <TextField label="邮件内容:" multiline rows={8} value={this.state.content   } onChange={this._handleInputChange.bind(this, "content")} />
                <PrimaryButton text="发送" allowDisabledFocus onClick={this._handleSend.bind(this)} />
            </Stack>
        );
    }
}   