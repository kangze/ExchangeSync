import * as React from 'react';

import { Text } from 'office-ui-fabric-react/lib/Text';

import { IconButton } from 'office-ui-fabric-react/lib/Button';

export default class CalendarItem extends React.Component<any, any>{

    private _handleCreateCalendar(e:any){
        (this.props as any).history.push("/createCalendar");
    }

    public render() {
        let imgUrl = "/img/1.jpeg";
        return (
            <div>
                <div style={{ width: "100%", height: 200, backgroundImage: 'url(' + imgUrl + ')', backgroundRepeat: "round" }}>
                    <div style={{ paddingLeft: "20%", paddingTop: 20 }}>
                        <Text variant="xLarge" style={{ color: "black" }}>2019年5月</Text>
                    </div>
                </div>

                <br />
                <div style={{ float: "left", width: "20%", textAlign: "center", height: 45 }}>
                    <Text variant="medium" style={{ color: "black" }}>周一</Text>
                    <br />
                    <Text variant="large" style={{ color: "black" }}>3</Text>
                </div>
                <div style={{ float: "left", width: "77%", backgroundColor: "#079848", borderRadius: 5, height: 45 }}>
                    <div style={{ paddingTop: 8, paddingLeft: 13 }}>
                        <Text variant="large" style={{ color: "white" }}>明天讨论PCWP相关的事情</Text>
                    </div>
                </div>

                <div style={{ position: "fixed", right: 20, bottom: 20 }}>
                    <IconButton
                        iconProps={{
                            iconName: 'CalendarSettings', styles: {
                                root: {
                                    color: "#0a61af",
                                    fontSize: 54,
                                }
                            }
                        }}
                        styles={{ root: { height: 64 } }}
                        title="Add"
                        ariaLabel="Add"
                        onClick={this._handleCreateCalendar.bind(this)}
                    />
                </div>
            </div>
        );
    }
}