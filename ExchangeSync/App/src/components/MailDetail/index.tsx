import * as React from 'react';
import { Stack, IStackTokens } from 'office-ui-fabric-react/lib/Stack';
import { Text } from 'office-ui-fabric-react/lib/Text';
import { IPersonaSharedProps, Persona, PersonaInitialsColor, PersonaSize } from 'office-ui-fabric-react/lib/Persona';
import { TooltipHost, DirectionalHint } from 'office-ui-fabric-react/lib/Tooltip';
import { CommandBarButton } from 'office-ui-fabric-react/lib/Button';
import { IOverflowSetItemProps, OverflowSet } from 'office-ui-fabric-react/lib/OverflowSet';
import { DefaultButton } from 'office-ui-fabric-react';

//import { ActionMenus } from "./action";

const ActionMenus = [
    {
        key: 'markReaded',
        iconProps: {
            iconName: "Mail"
        },
        name: '标记为未读',
    },
    {
        key: 'mark',
        iconProps: {
            iconName: "Calendar"
        },
        name: '标记',
    }, {
        key: "answer",
        iconProps: {
            iconName: "Calendar"
        },
        name: "答复",
    }, {
        key: "answer_all",
        iconProps: {
            iconName: "Calendar"
        },
        name: "全部答复",
    }, {
        key: "to",
        iconProps: {
            iconName: "Calendar"
        },
        name: "转发",
    }
];

const styles = {
    persona: {
        root: {
            float: "left",
            marginLeft: 10
        }
    },
    time: {
        display: "inline-block",
        float: "right",
        marginRight: 10
    } as React.CSSProperties,
    overflowItem: {
        root: {
            padding: 0
        }
    }
};

const stackTokens: IStackTokens = { childrenGap: 12 };


export default class SeparatorThemingExample extends React.Component<any, any> {

    constructor(props: any) {
        super(props);
        if (props.staticContext && props.staticContext.data) {
            let data = props.staticContext.data;
            this.state = {
                loading: false,
                title: data.title,
                sender: data.sender,
                senderLink: null,
                date: data.date,
                content: data.content,
            }
        } else if ((window as any).detail) {
            let data1 = (window as any).detail;
            this.state = {
                loading: false,
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

    public render(): JSX.Element {
        //let id=(this.props as any).match.params.mailId; 获取到的I
        if (this.state.loading)
            return (
                <div>正在加载</div>
            );
        return (
            <Stack tokens={stackTokens}>
                <div style={{ padding: 10, backgroundColor: "#eaeaea" }}>
                    <Text variant="large" >{this.state.title}</Text>
                </div>
                <div>
                    <Persona secondaryText={'kangze25@126.com'} text={this.state.sender} size={PersonaSize.size48} styles={styles.persona} />
                    <div style={styles.time}>
                        <Text variant="medium" >{this.state.date}</Text>
                        <OverflowSet
                            vertical
                            overflowItems={ActionMenus}
                            onRenderOverflowButton={this._onRenderOverflowButton}
                            onRenderItem={this._onRenderItem}
                            styles={styles.overflowItem}
                        />
                    </div>
                    <div style={{ clear: "both" }}></div>
                    <div style={{ padding: 20 }}>
                        <div dangerouslySetInnerHTML={{ __html: this.state.content }}></div>
                    </div>
                </div>
                <div style={{ position: "fixed", width: "100%", bottom: 0, backgroundColor: "#eaeaea" }}>
                    <DefaultButton
                        text="回复"
                        allowDisabledFocus
                        styles={{ root: { width: "100%" } }}
                    />
                </div>
            </Stack>
        );
    }

    private _onRenderOverflowButton = (overflowItems: any[] | undefined): JSX.Element => {
        return (
            <CommandBarButton
                styles={{ root: { padding: "12px" }, menuIcon: { fontSize: '16px' } }}
                menuIconProps={{ iconName: 'More' }}
                menuProps={{ items: overflowItems! }}
            />
        );
    };

    private _onRenderItem = (item: IOverflowSetItemProps): JSX.Element => {
        return (
            <TooltipHost content={item.name} calloutProps={{ directionalHint: DirectionalHint.rightCenter, beakWidth: 12 }}>
                <CommandBarButton styles={{ root: { padding: '10px' } }} iconProps={{ iconName: item.icon }} onClick={item.onClick} />
            </TooltipHost>
        );
    };
}