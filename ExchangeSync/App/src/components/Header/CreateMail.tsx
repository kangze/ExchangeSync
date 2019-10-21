import * as React from 'react';
import axios from "axios";
import { IconButton } from 'office-ui-fabric-react/lib/Button';
import { Panel, PanelType } from 'office-ui-fabric-react/lib/Panel';
import { DefaultButton, PrimaryButton } from 'office-ui-fabric-react';
import { Depths } from '@uifabric/fluent-theme/lib/fluent/FluentDepths';
import { Text } from 'office-ui-fabric-react/lib/Text';

const styles = {
    root: {
        height: 48,
        backgroundColor: "#0078d4",
    } as React.CSSProperties,
}

export default class CreateMail extends React.Component<any, any>{

    private _handleCancel(e: any) {
        e.preventDefault();
        (this.props as any).history.goBack();
    }

    private _handleSend(e: any) {
        e.preventDefault();
        let state = (window as any).content;
        state["content"] = state.zxEditor.getHtml();
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
            mailId: state.mailId,
            title: state["title"],
            content: state["content"],
            reciver: state["reciver"],
            copyTo: state["copyto"] ? state["copyto"].map((item: any) => item.key) : null
        };
        axios.post("/mail/reply", data).then(reponse => {
            if (reponse.data.success)
                alert('回复成功');
            (this.props as any).history.push("/sended");
        })
    }

    public render() {
        return (
            <div style={styles.root}>
                <div style={{ float: "left" }}>
                    <IconButton onClick={this._handleCancel.bind(this)} styles={{ root: { height: 48, width: 64 }, icon: { fontSize: 21, color: "white" } }} iconProps={{ iconName: 'Cancel' }} title="取消" ariaLabel="取消" />
                </div>
                <div style={{ float: "left" }}>
                    <Text variant="xLarge" style={{ color: "white" }}>新建邮件</Text>
                    <br />
                    <Text variant="medium" style={{ color: "white" }}>kangze25@126.com</Text>
                </div>
                <div style={{ float: "right" }}>
                    <IconButton onClick={this._handleSend.bind(this)} styles={{ root: { height: 48, width: 64 }, icon: { fontSize: 32, color: "white" } }} iconProps={{ iconName: 'Send' }} title="取消" ariaLabel="取消" />
                </div>
            </div>
        );
    }
}