import * as React from 'react';
import { IPersonaSharedProps, Persona, PersonaInitialsColor, PersonaSize } from 'office-ui-fabric-react/lib/Persona';
import { Stack } from 'office-ui-fabric-react/lib/Stack';
import { Text } from 'office-ui-fabric-react/lib/Text';
import { IconButton } from 'office-ui-fabric-react/lib/Button';


const examplePersona: IPersonaSharedProps = {
    imageInitials: "M",
    size: PersonaSize.size48,
};

const styles = {
    root: {
        marginTop: 10,
        marginLeft: 10,
        float: "left"
    },
    details: {
        //backgroundColor:"red",
        //marginTop: -25
    }
}

export interface IMailItemGroupedProps {
    items: IMailItemGroupProps[]
}

export interface IMailItemGroupProps {
    groupTitle: string,
    items: IMailItemProps[]
}

export interface IMailItemProps {
    /**
     * 发件人
     */
    sender: string,

    /**
     * 邮件标题
     */
    title: string,

    /**
     * 邮件的ID,用于查看详情
     */
    mailId: string,

    /**
     * 邮件简短的描述信息
     */
    description: string

    /**
     * 指示邮件是否被读取
     */
    readed: boolean,

    /**
     * 邮件时间,是已经被服务器分号组的
     */
    date: string,
}

export default class MailItem extends React.PureComponent {

    public handleClick(item: IMailItemProps) {
        //console.log(this.props);
        (this.props as any).history.push("/detail/" + item.mailId);
    }

    handleCreate() {
        (this.props as any).history.push("/create");
    }

    public renderItem(items: IMailItemProps[]) {
        return items.map(item => {
            let style = undefined;
            //没有读取的邮件，所有字体设置为粗体
            if (!item.readed) style = { fontWeight: "bold" } as React.CSSProperties;
            return (
                <Stack>
                    <Persona
                        onClick={this.handleClick.bind(this, item)}
                        imageInitials={item.sender[0]}
                        initialsColor={PersonaInitialsColor.magenta}
                        size={PersonaSize.size40}
                        styles={styles}>
                        <div>
                            <div>
                                <Text key={item.sender} style={style} variant="medium" nowrap block>{item.sender}</Text>
                            </div>

                            <div>
                                <Text key={item.title} style={style} variant="medium" nowrap block>{item.title}</Text>
                            </div>
                            <div>
                                <Text key={item.description} style={style} variant="medium" nowrap block>{item.description}</Text>
                            </div>

                            <div style={{ position: "absolute", right: 25, top: 0 }}>
                                <Text key={item.date} variant="medium" style={{ color: "#3f8d57" }} nowrap block>{item.date}</Text>
                            </div>
                        </div>
                    </Persona>
                    <hr style={{ backgroundColor: "#927a7a", marginTop: 10, height: 1, border: "none" }} />
                </Stack>
            );
        })
    }

    public render(): JSX.Element {
        var items = [{
            sender: "kangze",
            title: "I Want You",
            mailId: "id1",
            description: "Someone want to look for you to join some party!!",
            date: "周三"
        },
        {
            sender: "Xiangmingliang",
            title: "I Want You",
            mailId: "id2",
            description: "Someone want to look for you to join some party!!",
            readed: true,
            date: "周三"
        }
        ];
        var groups = [
            {
                groupTitle: "本周",
                items
            },
            {
                groupTitle: "上周",
                items
            }
        ]
        return (
            <Stack tokens={{ childrenGap: 10 }}>
                {groups.map(group => {
                    return (
                        <div>
                            <div style={{ marginLeft: 15, marginTop: 10 }}>
                                <Text key={"group1"} variant="medium" nowrap block>上周</Text>
                            </div>
                            {this.renderItem(items as any)}
                        </div>
                    );
                })}
                <div style={{ position: "fixed", right: 20, bottom: 20 }}>
                    <IconButton
                        iconProps={{
                            iconName: 'EditMail', styles: {
                                root: {
                                    color: "#0a61af",
                                    fontSize: 48,
                                },
                                imageContainer: {
                                    height: 128
                                }
                            }
                        }}
                        title="Add"
                        ariaLabel="Add"
                        onClick={this.handleCreate.bind(this)}
                    />
                </div>
            </Stack>
        );
    }
}