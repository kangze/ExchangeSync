import * as React from 'react';

import { IconButton } from 'office-ui-fabric-react/lib/Button';
import { Panel, PanelType } from 'office-ui-fabric-react/lib/Panel';
import { DefaultButton, PrimaryButton } from 'office-ui-fabric-react';
import { Depths } from '@uifabric/fluent-theme/lib/fluent/FluentDepths';

require("./index.css");

const styles = {
    root: {
        height: 48,
        backgroundColor: "#F5F3F1",
    } as React.CSSProperties,
    logodiv: {
        paddingTop: 6,
        textAlign: "left",
        fontSize: 15,
        "float": "left",
        width: "15%",
    } as React.CSSProperties,
    appsdiv: {
        width: "15%",
        textAlign: "left",
        "float": "left",

    } as React.CSSProperties,
    appIcon: {
        width: 48,
        height: 48
    },
    img: {
        height: 36
    } as React.CSSProperties,
    text: {
        textAlign: "center",
        fontSize: 22,
        height: 48,
        float: "left",
        width: "70%",

    } as React.CSSProperties,
    textp: {
        margin: 10,
        color: "#D53A00",
        textAlign: "center",
    } as React.CSSProperties,
}


export interface IPanelSmallLeftExampleState {
    showPanel: boolean;
}

const menus = [
    {
        name: "收件箱",
        icon: "MailSolid",
        key: "inBox",
        path: "/"
    },
    {
        name: "已经发送",
        icon: "MailForward",
        key: "Forwart",
        path: "/sended"
    }, {
        name: "草稿",
        icon: "EditMail",
        key: "editMail",
        path: "/draft"
    },

]

export default class Header extends React.Component<any, any> {

    public state = {
        showPanel: false
    };

    public handleMenuClick(path: string) {
        (this.props as any).history.push(path);
        this.setState({ showPanel: false });
    }

    public render() {
        return (
            <div style={styles.root}>
                <div style={styles.appsdiv}>
                    <IconButton
                        styles={{ root: styles.appIcon }}
                        iconProps={{ iconName: 'WaffleOffice365' }} title="Apps" ariaLabel="Add"
                        onClick={this._showPanel}
                    />

                </div>

                <div style={styles.logodiv} >
                    <img style={styles.img} src="https://bi.scrbg.com/images/logo.png" />
                </div>
                <div style={styles.text}>
                    <p style={styles.textp}>
                        <span>列表结合测试</span>
                    </p>
                </div>
                <div style={{ clear: "both" }}></div>
                <Panel
                    isOpen={this.state.showPanel}
                    type={PanelType.smallFixedNear}
                    onDismiss={this._hidePanel}
                    isLightDismiss={true}
                    headerText="kangze25@126.com">

                    {menus.map(item => {
                        return (
                            <DefaultButton
                                text={item.name}
                                allowDisabledFocus
                                styles={{
                                    root: {
                                        border: "none",
                                        width: "100%",
                                        marginTop: 20
                                    }
                                }}
                                iconProps={{
                                    iconName: item.icon
                                }}
                                onClick={this.handleMenuClick.bind(this, item.path)}
                            />
                        );
                    })}
                </Panel>
            </div>
        );
    }

    private _showPanel = () => {
        this.setState({ showPanel: true });
    };

    private _hidePanel = () => {
        this.setState({ showPanel: false });
    };
}