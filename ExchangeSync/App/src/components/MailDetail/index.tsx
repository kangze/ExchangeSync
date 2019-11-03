import * as React from 'react';
import { Stack, IStackTokens } from 'office-ui-fabric-react/lib/Stack';
import { Text } from 'office-ui-fabric-react/lib/Text';
import { IPersonaSharedProps, Persona, PersonaInitialsColor, PersonaSize } from 'office-ui-fabric-react/lib/Persona';
import { TooltipHost, DirectionalHint } from 'office-ui-fabric-react/lib/Tooltip';
import { CommandBarButton } from 'office-ui-fabric-react/lib/Button';
import { IOverflowSetItemProps, OverflowSet } from 'office-ui-fabric-react/lib/OverflowSet';
import { DefaultButton, IContextualMenuProps,PrimaryButton } from 'office-ui-fabric-react';
import { Spinner } from 'office-ui-fabric-react/lib/Spinner';
import axios from "axios";

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

function GetMenus(mailId: string, attachmentid: string, name: string): IContextualMenuProps {
    var items = [];
    items.push({
        key: 'emailMessage',
        text: '下载',
        iconProps: { iconName: 'Mail' },
        href: "/mail/DownloadAttachment?mailId=" + encodeURIComponent(mailId) + "&attachmentid=" + encodeURIComponent(attachmentid) + "&attachmentName=" + name
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

    componentDidMount() {
        if (!this.state.loading)
            return;
        //这里开始加载具体的邮件内容
        let self = this;
        let mailId = this.props.match.params.mailId;
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

    public render(): JSX.Element {
        //let id=(this.props as any).match.params.mailId; 获取到的I
        var canReply = (window as any).location.pathname == "/";
        if (this.state.loading)
            return <Spinner styles={{ root: { marginTop: 40 } }} label="正在加载数据..." />
        let mailid = this.state.mailId;
        return (
            <Stack tokens={stackTokens}>
                <div style={{ padding: 10, backgroundColor: "#eaeaea" }}>
                    <Text variant="large" >{this.state.title}</Text>
                </div>
                <div>
                    <Persona secondaryText={this.state.senderLink} text={this.state.sender} size={PersonaSize.size48} styles={Styles.persona} />
                    <div style={Styles.time}>
                        <Text variant="medium" >{this.state.date}</Text>
                        <OverflowSet
                            vertical
                            overflowItems={this.getMenu(this.state.folderName, this.state.mailId)}
                            onRenderOverflowButton={this._onRenderOverflowButton}
                            onRenderItem={this._onRenderItem}
                            styles={Styles.overflowItem}
                        />
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
                                    menuProps={GetMenus(mailid, attach.id, attach.name)}
                                />
                            )
                        }
                    </div>
                    <div>

                        <iframe src={"/mail/getcontent?mailId=" + mailid} width={"100%"} height={600} style={{ border: "none" }}></iframe>
                    </div>
                </div>
                {
                    this.state.folderName === "inbox" ?
                        <div style={{ position: "fixed", width: "100%", bottom: 0, backgroundColor: "#eaeaea" }}>
                            <PrimaryButton
                                text="回复"
                                allowDisabledFocus
                                styles={{ root: { width: "100%" } }}
                                onClick={this.handleReply.bind(this, this.state.mailId)}
                            />
                        </div> : undefined
                }
            </Stack>
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