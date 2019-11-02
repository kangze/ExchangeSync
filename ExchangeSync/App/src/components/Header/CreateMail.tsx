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
    constructor(props: any) {
        super(props);
        if (props.staticContext && props.staticContext.user) {
            let user = props.staticContext.user;
            this.state = {
                user: user
            }
        } else if ((window as any).user) {
            let user = (window as any).user;
            this.state = {
                user: user
            }
        }
    }
    private _handleCancel(e: any) {
        e.preventDefault();
        (this.props as any).history.goBack();
    }

    private _handleSend(e: any) {
        e.preventDefault();
        if ((window as any).createMail) {
            this._handleMailCreateSend();
        } else {
            this._handleCalendarSend();
        }
    }

    private _handleCalendarSend() {
        var state = (window as any).createCalendar;
        if (!state.title) {
            alert("请输入会议标题!");
            return;
        }
        if (!state.location) {
            alert("请输入会议地点!");
            return;
        }

        if (!state.start) {
            alert("请输入会议开始时间");
            return;
        }
        if (!state.FullDay && !state.startTime) {
            alert("请输入会议开始时间");
            return;
        }
        if (!state.FullDay && (!state.end || !state.endTime)) {
            alert("请输入会议结束时间");
            return;
        }
        if (!state.attendees || state.attendees.length == 0) {
            alert("请输入会议参与人员");
            return;
        }
        var content = (window as any).zxEditor.getHtml();
        var s = (window as any).zxEditor.getHtml(false);
        if (!s) {
            alert("请输输入会议内容");
            return;
        }
        var data = {
            title: state.title,
            body: content,
            location: state.location,
            start: state.start.toLocaleDateString(),
            startTime: state.startTime,
            end: !state.end ? "" : state.end.toLocaleDateString(),
            endTime: state.endTime,
            attendees: state.attendees ? state.attendees.map((item: any) => item.key) : null,
            AddToSkype: state.AddToSkype,
            FullDay: state.FullDay,
        };
        var formData = new FormData();
        if (data.title)
            formData.append("title", data.title);
        if (data.body)
            formData.append("body", data.body);
        if (data.location)
            formData.append("location", data.location);
        if (data.start)
            formData.append("start", data.start);
        if (data.startTime)
            formData.append("startTime", data.startTime);
        if (data.end)
            formData.append("end", data.end);
        if (data.endTime)
            formData.append("endTime", data.endTime);
        if (data.attendees)
            formData.append("attendees", data.attendees);
        if (data.AddToSkype)
            formData.append("AddToSkype", data.AddToSkype);
        if (data.FullDay)
            formData.append("FullDay", data.FullDay);
        var files = (window as any).document.getElementById("attachmentCanlendar").files;
        if (files.length > 0)
            for (let i = 0; i < files.length; i++) {
                formData.append("attachment", files[i]);
            }
        let self = this;
        axios.post("/Calendar/CreateAppointMent", formData).then(reponse => {
            if (reponse.data.success) {
                alert(self.props.message);
                delete (window as any).createCalendar;
                (self.props as any).history.push("/calendar");
            }
        }).catch(u => {
            alert("请稍后重新尝试");
        })
    }

    private _handleMailCreateSend() {
        let state = (window as any).content;
        state["content"] = (window as any).zxEditor.getHtml();
        console.log(state);
        var s = (window as any).zxEditor.getHtml(false);
        if (!s) {
            alert("请输入邮件内容");
            return;
        }
        if (!state["reciver"] || state["reciver"].length == 0) {
            alert("请选择收件人!");
            return;
        }
        if (!state["title"]) {
            alert("请输入邮件主题");
            return;
        }
        let data = {
            mailId: state.mailId,
            title: state["title"],
            content: state["content"],
            reciver: state["reciver"],
            copyTo: state["copyto"],
        };
        var formData = new FormData();
        if (data.mailId)
            formData.append("mailId", data.mailId);
        if (data.title)
            formData.append("title", data.title);
        if (data.content)
            formData.append("content", data.content);
        if (data.reciver) {
            for (let i = 0; i < data.reciver.length; i++) {
                formData.append("reciver", data.reciver[i]);
            }
        }

        if (data.copyTo) {
            for (let i = 0; i < data.copyTo.length; i++) {
                formData.append("copyTo", data.copyTo[i]);
            }
        }

        var files = (window as any).document.getElementById("attachmentMail").files;
        if (files.length > 0)
            for (let i = 0; i < files.length; i++) {
                formData.append("attachment", files[i]);
            }
        var self = this;
        axios.post("/mail/reply", formData).then(reponse => {
            if (reponse.data.success) {
                alert(self.props.message);
                delete (window as any).content;
                (self.props as any).history.push("/sended");
            }
        }).catch(error => {
            alert("请稍后重新尝试!");
        })
    }

    public render() {
        let userName = this.state.user.userName + "@scrbg.com";
        return (
            <div style={styles.root}>
                <div style={{ float: "left" }}>
                    <IconButton className="btnhover" onClick={this._handleCancel.bind(this)} styles={{ root: { height: 48, width: 64 }, icon: { fontSize: 21, color: "white" } }} iconProps={{ iconName: 'Cancel' }} title="取消" ariaLabel="取消" />
                </div>
                <div style={{ float: "left" }}>
                    <Text variant="xLarge" style={{ color: "white" }}>{this.props.title}</Text>
                    <br />
                    <Text variant="medium" style={{ color: "white" }}>{userName}</Text>
                </div>
                <div style={{ float: "right" }}>
                    <IconButton className="btnhover" onClick={this._handleSend.bind(this)} styles={{ root: { height: 48, width: 64 }, icon: { fontSize: 23, color: "white" } }} iconProps={{ iconName: 'Send' }} title="取消" ariaLabel="取消" />
                </div>
            </div>
        );
    }
}