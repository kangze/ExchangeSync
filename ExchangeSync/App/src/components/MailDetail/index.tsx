import * as React from 'react';
import { Stack, IStackTokens } from 'office-ui-fabric-react/lib/Stack';
import { Text } from 'office-ui-fabric-react/lib/Text';
import { IPersonaSharedProps, Persona, PersonaInitialsColor, PersonaSize } from 'office-ui-fabric-react/lib/Persona';
import { TooltipHost, DirectionalHint } from 'office-ui-fabric-react/lib/Tooltip';
import { CommandBarButton } from 'office-ui-fabric-react/lib/Button';
import { IOverflowSetItemProps, OverflowSet } from 'office-ui-fabric-react/lib/OverflowSet';
import { DefaultButton, IContextualMenuProps, PrimaryButton } from 'office-ui-fabric-react';
import { Spinner } from 'office-ui-fabric-react/lib/Spinner';
import axios from "axios";
import { CommandBar, ICommandBarItemProps } from 'office-ui-fabric-react/lib/CommandBar';

import { ActionMenus, Styles, InBoxMenu, SentMenu, DraftMenu } from "./action";
import { Icon } from 'office-ui-fabric-react/lib/Icon';








const stackTokens: IStackTokens = { childrenGap: 12 };

const menuProps: IContextualMenuProps = {
    items: [
        {
            key: 'emailMessage',
            text: '下载',
            iconProps: { iconName: 'Mail' }
        }
    ]
};

function GetMenus(mailId: string, attachmentid: string, name: string, number: string): IContextualMenuProps {
    var items = [];
    items.push({
        key: 'emailMessage',
        text: '下载',
        iconProps: { iconName: 'Mail' },
        href: "/mail/DownloadAttachment?mailId=" + encodeURIComponent(mailId) + "&attachmentid=" + encodeURIComponent(attachmentid) + "&attachmentName=" + name + "&userName=" + number
    })
    return { items };
}

export default class SeparatorThemingExample extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        if (props.staticContext && props.staticContext.data) {
            let data = props.staticContext.data;
            this.state = {
                loading: false,
                mailId: data.mailId,
                title: data.title,
                sender: data.sender.name,
                senderLink: data.sender.address,
                date: data.date,
                content: data.content,
                attachments: data.attachments,
                folderName: data.folderName
            }
        } else if ((window as any).data) {
            let data1 = (window as any).data;
            delete (window as any).data;
            this.state = {
                loading: false,
                mailId: data1.mailId,
                title: data1.title,
                sender: data1.sender.name,
                senderLink: data1.sender.address,
                date: data1.date,
                content: data1.content,
                attachments: data1.attachments,
                folderName: data1.folderName
            }
        } else {
            this.state = { loading: true };
        }

    }

    public handleReply(mailId: string) {
        (this.props as any).history.push("/reply/" + encodeURIComponent(mailId));
    }

    handle_Reply() {
        (this.props as any).history.push("/reply/" + this.props.mailId);
    }

    componentDidMount() {
        if (!this.state.loading)
            return;
        //这里开始加载具体的邮件内容
        let mailId = "";
        if (this.props.mailId)
            mailId = this.props.mailId;
        else
            mailId = this.props.match.params.mailId;
        this.getDetail(mailId);
    }

    componentWillReceiveProps(nextProps: any) {
        let mailId = "";
        if (nextProps.mailId)
            mailId = nextProps.mailId;
        else
            mailId = nextProps.match.params.mailId;
        this.setState({ loading: true });
        this.getDetail(mailId);
    }

    getDetail(mailId: any) {
        let self = this;

        axios.get("/mail/GetMail?mailId=" + mailId).then(response => {
            let data = response.data;
            self.setState({
                loading: false,
                mailId: data.mailId,
                title: data.title,
                sender: data.sender.name,
                senderLink: data.sender.address,
                date: data.date,
                content: data.content,
                attachments: data.attachments,
                folderName: data.folderName,
            })
        })
    }

    handle_load() {
        (this.refs.iframeContent as any).height = 1000;
    }

    public getMenu(name: string, mailId: string) {
        if (name == "inbox") {
            let menu = InBoxMenu.map(u => {
                u.onClick = u.onClick.bind(this, mailId);
                return u;
            })
            return menu;
        } else if (name = "sent") {
            let menu = SentMenu.map(u => {
                u.onClick = u.onClick.bind(this, mailId);
                return u;
            })
            return menu;
        } else {
            let menu = DraftMenu.map(u => {
                u.onClick = u.onClick.bind(this, mailId);
                return u;
            })
            return menu;
        }
    }

    handleCreate() {
        (this.props as any).history.push("/create");
    }

    handle_delete() {
        axios.post("/mail/Delete?mailid=" + this.props.mailId).then(response => {
            if (response.data.success) {
                alert("删除成功!");
                (window as any).location.reload();
            }
        }).catch(u => {
            alert("请稍后重新尝试");
        })
    }

    handle_setRead() {
        axios.post("/mail/setunreade?mailid=" + this.props.mailId).then(response => {
            if (response.data.success) {
                alert("已经标记为未读!");
            }
        }).catch(u => {
            alert("请稍后重新尝试");
        })
    }

    private _handleCreateCalendar(e: any) {
        (this.props as any).history.push("/createCalendar");
    }

    public render(): JSX.Element {
        if (this.props.mailId == "un") {
            return (
                <Stack tokens={stackTokens}>
                    <div className="ms-hiddenSm">
                        {this.props.calendar ?
                            <CommandBar
                                styles={{
                                    root: {
                                        backgroundColor: "#c9d7e6",
                                        padding: 0
                                    }
                                }}
                                items={[
                                    {
                                        key: 'up',
                                        text: '新建会议',
                                        iconProps: { iconName: 'Add' },
                                        onClick: this._handleCreateCalendar.bind(this),
                                        buttonStyles: {
                                            root: {
                                                backgroundColor: "#c9d7e6"
                                            }
                                        }
                                    }
                                ]}
                            />
                            :
                            <CommandBar
                                styles={{
                                    root: {
                                        backgroundColor: "#c9d7e6",
                                        padding: 0
                                    }
                                }}
                                items={[
                                    {
                                        key: 'up',
                                        text: '新建邮件',
                                        iconProps: { iconName: 'Add' },
                                        onClick: this.handleCreate.bind(this),
                                        buttonStyles: {
                                            root: {
                                                backgroundColor: "#c9d7e6"
                                            }
                                        }
                                    }
                                ]}
                            />}

                    </div>
                </Stack>
            );
        }
        if (this.state.loading)
            return <Spinner styles={{ root: { marginTop: 40 } }} label="正在加载数据..." />
        let mailid = this.state.mailId;
        let userName = "";
        let wechat = null;
        if (window)
        {
            userName = (window as any).user.userName;
            wechat= (window as any).user.wechat;
        }
            
        return (
            <Stack tokens={stackTokens}>
                <div className="ms-hiddenSm">
                    {this.props.calendar ? <CommandBar
                        styles={{
                            root: {
                                backgroundColor: "#c9d7e6",
                                padding: 0
                            }
                        }}
                        items={[
                            {
                                key: 'up',
                                text: '新建会议',
                                iconProps: { iconName: 'Add' },
                                onClick: this._handleCreateCalendar.bind(this),
                                buttonStyles: {
                                    root: {
                                        backgroundColor: "#c9d7e6"
                                    }
                                }
                            }
                        ]}
                    /> :
                        <CommandBar
                            styles={{
                                root: {
                                    backgroundColor: "#c9d7e6",
                                    padding: 0
                                }
                            }}
                            items={[
                                {
                                    key: 'up',
                                    text: '新建邮件',
                                    iconProps: { iconName: 'Add' },
                                    onClick: this.handleCreate.bind(this),
                                    buttonStyles: {
                                        root: {
                                            backgroundColor: "#c9d7e6"
                                        }
                                    }
                                },
                                {
                                    key: 'share',
                                    text: '删除',
                                    iconProps: { iconName: 'Delete' },
                                    buttonStyles: {
                                        root: {
                                            backgroundColor: "#c9d7e6"
                                        }
                                    },
                                    onClick: this.handle_delete.bind(this),
                                },
                                {
                                    key: 'share1',
                                    text: '设置为未读',
                                    iconProps: { iconName: 'Read' },
                                    buttonStyles: {
                                        root: {
                                            backgroundColor: "#c9d7e6"
                                        }
                                    },
                                    onClick: this.handle_setRead.bind(this),
                                },
                                {
                                    key: 'reply',
                                    text: '回复',
                                    iconProps: { iconName: 'Reply' },
                                    buttonStyles: {
                                        root: {
                                            backgroundColor: "#c9d7e6"
                                        }
                                    },
                                    onClick: this.handle_Reply.bind(this),
                                },
                            ]}
                        />
                    }
                </div>
                <div style={{ padding: 10, backgroundColor: "#eaeaea" }}>
                    <Text variant="large" >{this.state.title}</Text>
                </div>
                <div>
                    <Persona secondaryText={this.state.senderLink} text={this.state.sender} size={PersonaSize.size48} styles={Styles.persona} />
                    <div style={Styles.time}>
                        <Text variant="medium" >{this.state.date}</Text>
                        <div className="ms-hiddenMdUp">
                            <OverflowSet
                                vertical
                                overflowItems={this.getMenu(this.state.folderName, this.state.mailId)}
                                onRenderOverflowButton={this._onRenderOverflowButton}
                                onRenderItem={this._onRenderItem}
                                styles={Styles.overflowItem}
                            />
                        </div>
                    </div>
                    <div style={{ clear: "both" }}></div>
                    <div>
                        {
                            this.state.attachments.map((attach: any) =>
                                <DefaultButton
                                    styles={{ root: { border: "none", maxWidth: "75%", minHeight: 42, height: "auto", wordWrap: "break-word", wordBreak: "break-all" }, }}
                                    iconProps={{ iconName: "PictureFill" }}
                                    text={attach.name}
                                    split
                                    splitButtonAriaLabel="See 2 options"
                                    aria-roledescription="split button"
                                    menuProps={GetMenus(mailid, attach.id, attach.name, userName)}
                                />
                            )
                        }
                    </div>
                    <div>

                        <iframe ref="iframeContent" height={"700"} src={"/mail/getcontent?mailId=" + mailid} width={"100%"} style={{ border: "none" }}></iframe>
                    </div>
                </div>
                {
                    this.state.folderName === "inbox" && !wechat ?
                        <div className="ms-hiddenMdUp" style={{ position: "fixed", width: "100%", bottom: 0, backgroundColor: "#eaeaea" }}>
                            <PrimaryButton
                                text="回复"
                                allowDisabledFocus
                                styles={{ root: { width: "100%" } }}
                                onClick={this.handleReply.bind(this, this.state.mailId)}
                            />
                        </div> : undefined
                }
            </Stack >
        );
    }

    private _onRenderOverflowButton = (overflowItems: any[] | undefined): JSX.Element => {

        return (
            <CommandBarButton
                styles={{ root: { padding: "10px" }, menuIcon: { fontSize: '16px' }, label: { textAlign: "center" } }}
                menuIconProps={{ iconName: 'More' }}
                menuProps={{ items: overflowItems }}
            />
        );
    };

    private _onRenderItem = (item: IOverflowSetItemProps): JSX.Element => {

        return (
            <TooltipHost content={item.name} calloutProps={{ directionalHint: DirectionalHint.rightCenter, beakWidth: 12 }}>
                <CommandBarButton styles={{ root: { padding: '100px', backgroundColor: "red" } }} iconProps={{ iconName: "Mail" }} onClick={item.onClick} />
            </TooltipHost>
        );
    };
}