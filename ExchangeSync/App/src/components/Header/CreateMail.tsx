import * as React from 'react';

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
        console.log((window as any).content);
        console.log((window as any).content.zxEditor.getHtml());
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