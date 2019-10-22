import * as React from 'react';
import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { Stack, IStackProps } from 'office-ui-fabric-react/lib/Stack';
import { PrimaryButton, Label,DefaultButton } from 'office-ui-fabric-react';
import axios from "axios";
import { Spinner } from 'office-ui-fabric-react/lib/Spinner';

import { TagPicker, IBasePicker, ITag } from 'office-ui-fabric-react/lib/Pickers';

declare var ZxEditor: any;

export interface IMailCreateProps {
    reply: boolean,
    mailId: string,
}

export default class MailCreate extends React.Component<IMailCreateProps, any> {

    constructor(props: IMailCreateProps) {
        super(props);
        var cporps = props as any;
        if (cporps.staticContext && cporps.staticContext.data) { //服务器渲染(用于回复其他人的邮件)
            let data = cporps.staticContext.data;
            this.state = {
                mailId: data.mailId,
                reply: true,
                replyloading: false,
                title: "[回复]" + data.title,
                content: "",
                reciver: data.sender.address,
                reciverName: data.sender.name,
                copyto: "",
            }
        } else if ((window as any).data) {         //客户端二次渲染(用于回复其他人的邮件)
            let data = (window as any).data;
            delete (window as any).data;
            this.state = {
                mailId: data.mailId,
                reply: true,
                replyloading: false,
                title: "[回复]" + data.title,
                content: "",
                reciver: data.sender.address,
                reciverName: data.sender.name,
                copyto: "",
            }
        } else {                                    //客户端无状态渲染,表示从别的路由过来的
            this.state = {
                mailId: (this.props as any).match.params.mailId,
                reply: (this.props as any).match.params.mailId ? true : false,
                replyloading: (this.props as any).match.params.mailId ? true : false,
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
        this.setState({ [name]: items.map(u => u.key) });
        console.log(this);
    }

    private _handleInputChange(name: string, e: any) {
        this.setState({
            [name]: e.target.value
        })
    }

    componentDidUpdate() {
        (window as any).content = this.state;
    }

    componentDidMount() {
        let self = this;
        (window as any).content = true;
        if (!this.state.reply) {
            var zxEditor = new ZxEditor('#e', {
                fixed: true,
                placeholder: "点击编辑..."
            });
            zxEditor.on('change', function () {
                var content = self.state.zxEditor.getContent();
                self.setState({ content });
            });
            self.setState({ zxEditor });
        }
        if (this.state.reply) {
            let self = this;
            axios.get("/mail/GetMail?mailId=" + this.state.mailId).then(response => {
                let data = response.data;
                self.setState({
                    reply: true,
                    replyloading: false,
                    title: "[回复]" + data.title,
                    content: "",
                    reciver: [data.sender.address],
                    reciverName: data.sender.name,
                    copyto: "",
                });
                var zxEditor = new ZxEditor('#e', {
                    fixed: true,
                    placeholder: "点击编辑..."
                });
                zxEditor.on('change', function () {
                    var content = self.state.zxEditor.getContent();
                    self.setState({ content });
                })
                self.setState({ zxEditor });
            });
        }
    }


    public render() {
        if (this.state.reply && this.state.replyloading) {
            return (
                <Spinner styles={{ root: { marginTop: 40 } }} label="正在加载数据..." />
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
                    selectedItems={this.state.reciver && this.state.reply ? [{ key: this.state.reciver[0], name: this.state.reciverName }] : undefined}
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
                <TextField iconProps={{
                    iconName: 'LocationDot', styles: {
                        root: {
                            fontSize: 16,
                        }
                    }
                }} label="主题:" underlined value={this.state.title} onChange={this._handleInputChange.bind(this, "title")} />
                <TextField id="attachmentMail" multiple label="附件:" underlined  type="file"/>
                <Label>&nbsp;&nbsp;&nbsp;邮件内容:</Label>
                <div id="e" ref="e"></div>
            </Stack>
        );
    }
}   