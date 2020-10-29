import React, { useEffect, useContext, useState } from "react";
import { useLocation, useHistory } from 'react-router-dom';
import { Button } from "reactstrap";
import { useForm } from "react-hook-form"
import * as Yup from 'yup';
import { yupResolver } from '@hookform/resolvers/yup';
import FormControlLabel from "@material-ui/core/FormControlLabel";
import Checkbox from "@material-ui/core/Checkbox";
import { Box } from "@material-ui/core";
import Paper from "@material-ui/core/Paper";
import { ArticleTagContext } from "../../providers/ArticleTagProvider";
import { TagContext } from "../../providers/TagProvider";



const ArticleTagEdit = (articleId) => {

    const [article, setArticle] = useState("");
    const [isLoading, setIsLoading] = useState("true");
    const { articleTags, getTagsByArticleId, addArticleTag, deleteTagsByArticleId } = useContext(ArticleTagContext);
    const { tags, getAllTags, addTag } = useContext(TagContext)
    const location = useLocation();
    let articleIdnew = parseInt(location.pathname.split("/")[2]);
    console.log(articleIdnew)
    const history = useHistory();
    let newTags = [];

    const goBack = () => {
        history.push(`/articles/${articleId}`);
    }
    const schema = Yup.object().shape({
        tag_Ids: Yup.array()
            .transform(function (o, obj) {
                return o.filter(o => o);
            })
            .min(2, "")
    });

    const { register, handleSubmit, control, getValues, setValue } = useForm({
        resolver: yupResolver(schema),
        defaultValues: Object.fromEntries(
            tags.map((tag, i) => [
                `tag.id[${i}]`,
                articleTags.some(articleTag => articleTag.id === tags[i].id)
            ])
        )
    });




    const onSubmit = (data, evt) => {
        Object.keys(data).forEach(key => {
            if (data[key] !== false) {
                newTags.push(
                    {
                        "articleId": articleId,
                        "tagId": parseInt(data[key])
                    })
            };
        });
        deleteTagsByArticleId(articleId)
            .then((p) => {
                if (!newTags.length > 0) {
                    history.push(`/articles/${articleId}`)
                }
                else {
                    newTags.map((articleTag) => {
                        addArticleTag(articleTag)
                            .then(history.push(`/articles/${articleId}`))
                    })
                }

            })


    };


    useEffect(() => {
        getAllTags();
        getTagsByArticleId(articleId)
        setIsLoading(false);

    }, []);



    return !isLoading ? (

        <Box>

            <Paper

                component="form"
                onSubmit={handleSubmit(onSubmit)}
            >
                <div className="div__tags__list">
                    {tags.map((tag, i) => {
                        return (
                            <FormCheckBox
                                key={tag.id}
                                name={tag.title}
                                control={control}
                                setValue={setValue}
                                getValues={getValues}
                                value={tag.id}
                                label={tag.title}
                                register={register}
                                defaultValue={articleTags.some(articleTag => articleTag.id === tag.id)}
                            />
                        );
                    })}
                </div>

                <Button
                    variant="contained"
                    color="primary"
                    size="large"
                    type="submit"

                >
                    Save
            </Button>
                <Button
                    variant="contained"
                    color="secondary"
                    size="large"
                    onClick={goBack}

                >
                    Cancel
            </Button>
            </Paper>
        </Box>
    ) : null;
}

export const FormCheckBox = ({
    name,
    value,
    register,
    control,
    setValue,
    getValues,
    defaultValue
}) => {
    return (
        <FormControlLabel
            control={<Checkbox defaultChecked={defaultValue} />}
            name={name}
            inputRef={register}
            value={value}
            label={name}
        />
    );
};

export default ArticleTagEdit;