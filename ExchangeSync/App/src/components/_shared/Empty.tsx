import * as React from 'react';

import { Text } from 'office-ui-fabric-react/lib/Text';

export default class Empty extends React.Component<any, any>{

    public render() {
        let styles: any = { textAlign: "center", paddingLeft: "20%", paddingRight: "20%" };
        if (!this.props.calendar) {
            styles["marginTop"] = 64;
        }
        return (
            <div style={styles as React.CSSProperties}>

                <img style={{ width: "100%" }} src="/img/empty.jpg" />
                <Text variant="mediumPlus">暂无数据</Text>
            </div>
        );
    }
}