import * as React from 'react';

import { Text } from 'office-ui-fabric-react/lib/Text';
import { TextField } from 'office-ui-fabric-react/lib/TextField';
import { Stack, IStackProps } from 'office-ui-fabric-react/lib/Stack';
import axios from "axios";
import { PrimaryButton, Label } from 'office-ui-fabric-react';
import { Checkbox, ICheckboxProps } from 'office-ui-fabric-react/lib/Checkbox';
import { Dropdown, IDropdownOption } from 'office-ui-fabric-react/lib/Dropdown';
import { DatePicker, DayOfWeek, IDatePickerStrings } from 'office-ui-fabric-react';
import { TagPicker, IBasePicker, ITag } from 'office-ui-fabric-react/lib/Pickers';

const DayPickerStrings: IDatePickerStrings = {
    months: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],

    shortMonths: ['1月', '2月', '3月', '4月', '5月', '6月', '7月', '8月', '9月', '10月', '11月', '12月'],

    days: ['周日', '周一', '周二', '周三', '周四', '周五', '周六'],

    shortDays: ['日', '一', '二', '三', '四', '五', '六'],

    goToToday: 'Go to today',
    prevMonthAriaLabel: 'Go to previous month',
    nextMonthAriaLabel: 'Go to next month',
    prevYearAriaLabel: 'Go to previous year',
    nextYearAriaLabel: 'Go to next year',
    closeButtonAriaLabel: 'Close date picker'
};

declare var ZxEditor: any;

export default class CalendarCreate extends React.Component<any, any>{

    public constructor(props: any) {
        super(props);
        this.state = {
            firstDayOfWeek: DayOfWeek.Monday
        };
    }

    private _onFilterChanged = (filterText: string, tagList: ITag[]): ITag[] => {
        var promise = this._searchUser(filterText) as any;
        return promise;
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

    componentDidMount() {
        let self = this;
        this.loadJs("/js/zx-editor.min.js", function () {
            var zxEditor = new ZxEditor('#e', {
                fixed: true,
                placeholder: "点击编辑..."
            });
            zxEditor.on('change', function () {
                var content = self.state.zxEditor.getContent();
                self.setState({ content });
            })
            self.setState({ zxEditor });
        });
    }

    public loadJs(url: string, callback: any) {
        var script = document.createElement('script') as any;
        script.type = "text/javascript";
        if (typeof (callback) != "undefined") {
            if (script.readyState) {
                script.onreadystatechange = function () {
                    if (script.readyState == "loaded" || script.readyState == "complete") {
                        script.onreadystatechange = null;
                        callback();
                    }
                }
            } else {
                script.onload = function () {
                    callback();
                }
            }
        }
        script.src = url;
        document.body.appendChild(script);
    }

    private _formatDate(date?: Date): string {
        if (!date)
            return "还未选择时间格式";
        return date.toLocaleDateString();
    }

    private handleSelectDate(name?: string, date?: Date) {
        this.setState({ [name]: date });
    }

    private _handleInputChange(name: string, e: any) {
        if (name == "AddToSkype") {
            this.setState({
                [name]: !this.state.AddToSkype
            })
            return;
        }
        this.setState({
            [name]: e.target.value
        })
    }

    private _handleChange(name: string, items: ITag[]) {
        this.setState({ [name]: items });
    }

    componentDidUpdate() {
        (window as any).createCalendar = this.state;
    }

    public render() {
        const { firstDayOfWeek } = this.state;
        console.log(this);
        return (
            <Stack tokens={{ childrenGap: 20 }} styles={{ root: { width: "100%" } }}>
                <br />
                <TextField value={this.state.title} onChange={this._handleInputChange.bind(this, "title")} label="主题：" underlined />
                <TextField value={this.state.location} onChange={this._handleInputChange.bind(this, "location")} label="地点：" underlined />
                <Checkbox label="同步创建Skype会议" checked={this.state.AddToSkype} onChange={this._handleInputChange.bind(this, "AddToSkype")} />
                <DatePicker value={this.state.start} onSelectDate={this.handleSelectDate.bind(this, "start")} formatDate={this._formatDate} firstDayOfWeek={firstDayOfWeek} strings={DayPickerStrings} placeholder="选择日期..." ariaLabel="选择日期..." label="&nbsp;&nbsp;&nbsp;起始时间：" />
                <DatePicker value={this.state.end} onSelectDate={this.handleSelectDate.bind(this, "end")} formatDate={this._formatDate} firstDayOfWeek={firstDayOfWeek} strings={DayPickerStrings} placeholder="选择日期..." ariaLabel="选择日期..." label="&nbsp;&nbsp;&nbsp;结束时间：" />
                <Label>&nbsp;&nbsp;&nbsp;参会人员:</Label>
                <TagPicker
                    styles={{ root: { marginTop: "0px !important" }, text: { borderTopStyle: "none", borderRightStyle: "none", borderLeftStyle: "none" } }}
                    onResolveSuggestions={this._onFilterChanged}
                    getTextFromItem={this._getTextFromItem}
                    pickerSuggestionsProps={{
                        suggestionsHeaderText: '请输入姓名/邮件地址',
                        noResultsFoundText: '没有找到该用户'
                    }}
                    itemLimit={100}
                    onChange={this._handleChange.bind(this, "attendees")}
                />
                <Label>&nbsp;&nbsp;&nbsp;内容:</Label>
                <div id="e"></div>
            </Stack>
        );
    }
}