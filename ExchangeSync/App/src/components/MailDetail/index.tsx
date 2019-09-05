import * as React from 'react';
import { Stack, IStackTokens } from 'office-ui-fabric-react/lib/Stack';
import { Text } from 'office-ui-fabric-react/lib/Text';
import { IPersonaSharedProps, Persona, PersonaInitialsColor, PersonaSize } from 'office-ui-fabric-react/lib/Persona';
import { TooltipHost, DirectionalHint } from 'office-ui-fabric-react/lib/Tooltip';
import { CommandBarButton } from 'office-ui-fabric-react/lib/Button';
import { IOverflowSetItemProps, OverflowSet } from 'office-ui-fabric-react/lib/OverflowSet';


const stackTokens: IStackTokens = { childrenGap: 12 };

const noOp: () => void = function () { };

const examplePersona: IPersonaSharedProps = {
    secondaryText: 'Designer',
    tertiaryText: 'In a meeting',
    optionalText: 'Available at 4:00pm'
};
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
            // backgroundColor: "red"
        }
    }
};

const overflowItems = [
    {
        key: 'item4',
        icon: 'Mail',
        name: 'Overflow Link 1',
        onClick: noOp
    },
    {
        key: 'item5',
        icon: 'Calendar',
        name: 'Overflow Link 2',
        onClick: noOp
    }
];

export default class SeparatorThemingExample extends React.Component<{}, {}> {
    public render(): JSX.Element {
        return (
            <Stack tokens={stackTokens}>
                <div style={{ padding: 10, backgroundColor: "#eaeaea" }}>
                    <Text variant="large" >这是一个约会邀请</Text>
                </div>
                <div>
                    <Persona {...examplePersona} text="宋智洋" size={PersonaSize.size48} styles={styles.persona} />
                    <div style={styles.time}>
                        <Text variant="smallPlus" >2018-1-1</Text>
                        <OverflowSet
                            vertical
                            overflowItems={overflowItems}
                            onRenderOverflowButton={this._onRenderOverflowButton}
                            onRenderItem={this._onRenderItem}
                            styles={styles.overflowItem}
                        />
                    </div>
                    <div style={{ clear: "both" }}></div>
                    <div style={{ padding: 20 }}>
                        这是邮件内容的相关信息,比如今天实我的生日你会来吗？
                    </div>
                </div>
            </Stack>
        );
    }

    private _onRenderOverflowButton = (overflowItems: any[] | undefined): JSX.Element => {
        return (
            <CommandBarButton
                styles={{ root: { padding: '10px' }, menuIcon: { fontSize: '16px' } }}
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