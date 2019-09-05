import * as React from 'react';
import {
    DocumentCard,
    DocumentCardActivity,
    DocumentCardDetails,
    DocumentCardPreview,
    DocumentCardTitle,
    IDocumentCardPreviewProps,
    DocumentCardType,
    IDocumentCardActivityPerson
} from 'office-ui-fabric-react/lib/DocumentCard';
import { Stack, IStackTokens } from 'office-ui-fabric-react/lib/Stack';
import { getTheme } from 'office-ui-fabric-react/lib/Styling';

const theme = getTheme();
const { palette, fonts } = theme;
const people: IDocumentCardActivityPerson[] = [
    { name: 'Christian Bergqvist', profileImageSrc: '', initials: 'CB' }
];
const stackTokens: IStackTokens = { childrenGap: 20 };
const previewOutlookUsingIcon: IDocumentCardPreviewProps = {
    previewImages: [
        {
            previewIconProps: {
                iconName: 'OutlookLogo',
                styles: {
                    root: {
                        fontSize: fonts.superLarge.fontSize,
                        color: 'black',
                        backgroundColor: palette.neutralLighterAlt,
                    }
                }
            },
            width: 100
        }
    ],
    styles: {
        previewIcon: { backgroundColor: palette.neutralLighterAlt },
        root: {
            width: 100
        }
    }
};

export default class DocumentCardCompactExample extends React.PureComponent {
    public render(): JSX.Element {
        return (
            <Stack tokens={stackTokens}>
                {/* Email conversation */}
                <DocumentCard type={DocumentCardType.compact} onClickHref="http://bing.com">
                    <DocumentCardPreview {...previewOutlookUsingIcon} />
                    <DocumentCardDetails>
                        <DocumentCardTitle title="Conversation about takeaways from annual SharePoint conference" shouldTruncate={true} />
                        <DocumentCardActivity activity="Sent a few minutes ago" people={[people[0]]} />
                    </DocumentCardDetails>
                </DocumentCard>
            </Stack>
        );
    }
}