import * as React from 'react';
import { Stack, IStackTokens } from 'office-ui-fabric-react/lib/Stack';
import { Text } from 'office-ui-fabric-react/lib/Text';
import { IPersonaSharedProps, Persona, PersonaInitialsColor, PersonaSize } from 'office-ui-fabric-react/lib/Persona';
import { TooltipHost, DirectionalHint } from 'office-ui-fabric-react/lib/Tooltip';
import { CommandBarButton } from 'office-ui-fabric-react/lib/Button';
import { IOverflowSetItemProps, OverflowSet } from 'office-ui-fabric-react/lib/OverflowSet';
import { DefaultButton } from 'office-ui-fabric-react';
import { Spinner } from 'office-ui-fabric-react/lib/Spinner';
import axios from "axios";

import { ActionMenus, Styles } from "./action";
import { Icon } from 'office-ui-fabric-react/lib/Icon';








const stackTokens: IStackTokens = { childrenGap: 12 };


export default class SeparatorThemingExample extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        if (props.staticContext && props.staticContext.data) {
            let data = props.staticContext.data;
            this.state = {
                loading: false,
                mailId: data.mailId,
                title: data.title,
                sender: data.sender,
                senderLink: null,
                date: data.date,
                content: data.content,
            }
        } else if ((window as any).data) {
            let data1 = (window as any).data;
            delete (window as any).data;
            this.state = {
                loading: false,
                mailId: data1.mailId,
                title: data1.title,
                sender: data1.sender,
                senderLink: null,
                date: data1.date,
                content: data1.content,
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
                sender: data.senderName,
                senderLink: data.sender,
                date: data.date,
                content: data.content ? data.content : data.description,
            })
        })
    }

    public render(): JSX.Element {
        //let id=(this.props as any).match.params.mailId; 获取到的I
        if (this.state.loading)
            return <Spinner styles={{ root: { marginTop: 40 } }} label="正在加载数据..." />
        return (
            <Stack tokens={stackTokens}>
                <div style={{ padding: 10, backgroundColor: "#eaeaea" }}>
                    <Text variant="large" >{this.state.title}</Text>
                </div>
                <div>
                    <Persona secondaryText={'kangze25@126.com'} text={this.state.sender} size={PersonaSize.size48} styles={Styles.persona} />
                    <div style={Styles.time}>
                        <Text variant="medium" >{this.state.date}</Text>
                        <OverflowSet
                            vertical
                            overflowItems={ActionMenus}
                            onRenderOverflowButton={this._onRenderOverflowButton}
                            onRenderItem={this._onRenderItem}
                            styles={Styles.overflowItem}
                        />
                    </div>
                    <div style={{ clear: "both" }}></div>
                    <div>
                        <iframe srcDoc={this.state.content} width={"100%"} height={800} style={{ border: "none" }}></iframe>
                    </div>
                </div>
                <div style={{ position: "fixed", width: "100%", bottom: 0, backgroundColor: "#eaeaea" }}>
                    <DefaultButton
                        text="回复"
                        allowDisabledFocus
                        styles={{ root: { width: "100%" } }}
                        onClick={this.handleReply.bind(this, this.state.mailId)}
                    />
                </div>
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