import * as React from 'react';
import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { Stack, IStackProps } from 'office-ui-fabric-react/lib/Stack';
import { PrimaryButton, Label } from 'office-ui-fabric-react';
import axios from "axios";

import { TagPicker, IBasePicker, ITag } from 'office-ui-fabric-react/lib/Pickers';
import { Checkbox } from 'office-ui-fabric-react/lib/Checkbox';
import { mergeStyles } from 'office-ui-fabric-react/lib/Styling';

const rootClass = mergeStyles({
    maxWidth: 500
});



export default class MailCreate extends React.Component {

    constructor(props: any) {
        super(props);
    }

    private _onFilterChanged = (filterText: string, tagList: ITag[]): ITag[] => {
        var promise = this._searchUser(filterText) as any;
        return promise;
        //return [{ key: "kangze", name: "kangze" }];
    };

    private _getTextFromItem(item: ITag): string {
        return item.name;
    }

    private _searchUser(keyword: string): any {
        return new Promise(function (resolve, reject) {
            axios.get("/user/" + keyword).then(function (response) {
                var data = response.data;
                resolve(data);
            })
        })

    }

    public render() {
        return (
            <Stack tokens={{ childrenGap: 20 }} styles={{ root: { width: "100%" } }}>
                <Label>&nbsp;&nbsp;&nbsp;收件人：</Label>
                <TagPicker
                    styles={{ root: { marginTop: "0px !important" }, text: { borderTopStyle: "none", borderRightStyle: "none", borderLeftStyle: "none" } }}
                    onResolveSuggestions={this._onFilterChanged}
                    getTextFromItem={this._getTextFromItem}
                    pickerSuggestionsProps={{
                        suggestionsHeaderText: '请输入姓名/邮件地址',
                        noResultsFoundText: '没有找到该用户'
                    }}
                    itemLimit={1}
                />

                <Label>&nbsp;&nbsp;&nbsp;抄送(可选):</Label>
                <TagPicker
                    styles={{ root: { marginTop: "0px !important" }, text: { borderTopStyle: "none", borderRightStyle: "none", borderLeftStyle: "none" } }}
                    onResolveSuggestions={this._onFilterChanged}
                    getTextFromItem={this._getTextFromItem}
                    pickerSuggestionsProps={{
                        suggestionsHeaderText: '请输入姓名/邮件地址',
                        noResultsFoundText: '没有找到该用户'
                    }}
                    itemLimit={1}
                />
                <TextField label="主题:" underlined />
                <TextField label="邮件内容:" multiline rows={8} />
                <PrimaryButton text="发送" allowDisabledFocus />
            </Stack>
        );
    }
}   